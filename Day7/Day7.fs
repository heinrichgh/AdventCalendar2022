module AdventCalendar2022.Day7

open System.Text.RegularExpressions

let input =
    System.IO.File.ReadAllLines("Day7/input.txt")
    |> Array.toList


let (|ChangeDirIn|_|) (line: string) =
    if line.Substring(0, 4) = "$ cd" then
        Some(
            Regex
                .Match(line, @"\$ cd (.+)")
                .Groups[ 1 ]
                .ToString()
        )
    else
        None

let (|ChangeDirOut|_|) (line: string) =
    if line = "$ cd .." then
        Some()
    else
        None

let (|List|_|) (line: string) =
    if line.Substring(0, 4) = "$ ls" then
        Some()
    else
        None

let (|Directory|_|) (line: string) =
    let regexMatch =
        Regex.Match(line, "dir (.+)")

    if regexMatch.Success then
        Some(regexMatch.Groups[ 1 ].ToString())
    else
        None

let (|File|_|) (line: string) =
    let regexMatch =
        Regex.Match(line, "([0-9]+) (.+)")

    if regexMatch.Success then
        Some((int (regexMatch.Groups[ 1 ].ToString()), regexMatch.Groups[ 2 ].ToString()))
    else
        None

type FileDescriptor =
    { Name: string
      mutable Size: int
      ParentDirectory: DirectoryDescriptor option
      }

and DirectoryDescriptor =
    { Name: string
      mutable Size: int
      mutable FileDescriptors: FileDescriptor list
      mutable DirectoryDescriptors: DirectoryDescriptor list
      ParentDirectory: DirectoryDescriptor option
      }

// and FileDefinition =
//     | FileDescriptor of FileDescriptor
//     | DirectoryDescriptor of DirectoryDescriptor

let rec buildFileDefinitions lines (directory:DirectoryDescriptor) =
    match lines with
    | currentLine :: rest ->
        match currentLine with
        | ChangeDirOut ->
            let (Some parentDir) = directory.ParentDirectory
            buildFileDefinitions rest parentDir
        | ChangeDirIn dir ->
            let changingToDir =
                directory.DirectoryDescriptors
                |> List.filter (fun d -> d.Name = dir)
                |> List.head
            buildFileDefinitions rest changingToDir
        | List ->
            buildFileDefinitions rest directory
        | Directory dir ->
            let directoryDescriptors =
                         { Name = dir
                           Size = 0
                           FileDescriptors = []
                           DirectoryDescriptors = []
                           ParentDirectory = Some(directory)
                           }::directory.DirectoryDescriptors
            directory.DirectoryDescriptors <- directoryDescriptors
            buildFileDefinitions rest directory
        | File (size, fileName) ->
            let fileDefinitions =
                        {     Name = fileName
                              Size = size
                              ParentDirectory = Some(directory)
                            }::directory.FileDescriptors
            directory.FileDescriptors <- fileDefinitions
            
            buildFileDefinitions rest directory
        | x ->
            printfn $"Unknown line: '{x}'"
    | [] ->
        printfn $"Done"
        
let rec calculateSizes (directory:DirectoryDescriptor) =
    let filesSize = directory.FileDescriptors |> List.map(fun f -> f.Size) |> List.sum
    let directoriesSize = directory.DirectoryDescriptors |> List.map (fun d -> calculateSizes d) |> List.sum
    directory.Size <- filesSize + directoriesSize
    directory.Size
    
let rec findDirectories predicate (directory:DirectoryDescriptor) =
    let result = if predicate(directory) then Some directory else None
    result::(
        directory.DirectoryDescriptors
        |> List.map (findDirectories predicate)
        |> List.concat
        )
    
let rootDirectory =
    { Name = "/"
      Size = 0
      FileDescriptors = []
      DirectoryDescriptors = []
      ParentDirectory = None }
    
// Mutations... YaY!
buildFileDefinitions (input |> List.skip 1) rootDirectory
calculateSizes rootDirectory

let part1 = findDirectories (fun d -> d.Size <= 100000) rootDirectory
            |> List.choose id
            |> List.map (fun d -> d.Size)
            |> List.sum      

let diskSize = 70000000
let unusedSpace = diskSize - rootDirectory.Size
let spaceRequired = 30000000
let minimumSpaceToBeFreed = spaceRequired - unusedSpace
let smallestDirectoryForSpace = findDirectories (fun d -> d.Size >= minimumSpaceToBeFreed) rootDirectory
                                |> List.choose id
                                |> List.sortBy (fun d -> d.Size)
                                |> List.head
                                
let part2 = smallestDirectoryForSpace.Size