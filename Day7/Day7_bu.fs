module AdventCalendar2022.Day7_1
//
// open System.Text.RegularExpressions
//
// let input =
//     System.IO.File.ReadAllLines("Day7/test.txt")
//     |> Array.toList
//
//
// let (|ChangeDirIn|_|) (line: string) =
//     if line.Substring(0, 4) = "$ cd" then
//         Some(
//             Regex
//                 .Match(line, @"\$ cd (.+)")
//                 .Groups[ 1 ]
//                 .ToString()
//         )
//     else
//         None
//
// let (|ChangeDirOut|_|) (line: string) =
//     if line = "$ cd .." then
//         Some()
//     else
//         None
//
// let (|List|_|) (line: string) =
//     if line.Substring(0, 4) = "$ ls" then
//         Some()
//     else
//         None
//
// let (|Directory|_|) (line: string) =
//     let regexMatch =
//         Regex.Match(line, "dir (.+)")
//
//     if regexMatch.Success then
//         Some(regexMatch.Groups[ 1 ].ToString())
//     else
//         None
//
// let (|File|_|) (line: string) =
//     let regexMatch =
//         Regex.Match(line, "([0-9]+) (.+)")
//
//     if regexMatch.Success then
//         Some((int (regexMatch.Groups[ 1 ].ToString()), regexMatch.Groups[ 2 ].ToString()))
//     else
//         None
//
// type FileDescriptor =
//     { Name: string
//       Size: int
//       ParentDirectory: DirectoryDescriptor option
//       ParentDirectoryName: string option }
//
// and DirectoryDescriptor =
//     { Name: string
//       Size: int
//       FileDefinitions: FileDefinition list
//       ParentDirectory: DirectoryDescriptor option
//       ParentDirectoryName: string option }
//
// and FileDefinition =
//     | FileDescriptor of FileDescriptor
//     | DirectoryDescriptor of DirectoryDescriptor
//
// let rec buildFileDefinitions lines (fileDefinitions: FileDefinition list) (dirStack: string list) =
//     match lines with
//     | currentLine :: rest ->
//         match currentLine with
//         | ChangeDirOut ->
//             printfn $"CHANGE DIR OUT"            
//             let _::tail = dirStack
//             buildFileDefinitions rest fileDefinitions tail
//         | ChangeDirIn dir ->
//             printfn $"CHANGE DIR {dir}"
//
//             buildFileDefinitions rest fileDefinitions (dir::dirStack)
//         | List ->
//             printfn $"LIST"
//
//             buildFileDefinitions rest fileDefinitions dirStack
//         | Directory dir ->
//             printfn $"DIRECTORY {dir}"
//             let parentDir::_ = dirStack
//             let fileDefinitions =
//                 (DirectoryDescriptor
//                     { Name = dir
//                       Size = 0
//                       FileDefinitions = []
//                       ParentDirectory = None
//                       ParentDirectoryName = Some(parentDir) })
//                 :: fileDefinitions
//
//             buildFileDefinitions rest fileDefinitions dirStack
//         | File (size, fileName) ->
//
//             let parentDir::_ = dirStack
//             let fileDefinitions =
//                 (FileDescriptor
//                     { Name = fileName
//                       Size = size
//                       ParentDirectory = None
//                       ParentDirectoryName = Some(parentDir) })
//                 :: fileDefinitions
//
//             buildFileDefinitions rest fileDefinitions dirStack
//         | x ->
//             printfn $"Unknown line: '{x}'"
//             fileDefinitions
//     | [] ->
//         printfn $"Done"
//         fileDefinitions
//     
// let rootDirectory =
//     { Name = "/"
//       Size = 0
//       FileDefinitions = []
//       ParentDirectory = None
//       ParentDirectoryName = None }
//     
// let fileDefinitions = buildFileDefinitions (input |> List.skip 1) [ (DirectoryDescriptor rootDirectory) ] ["/"]
//
// let withParents parentName =
//     fileDefinitions
//     |> List.filter (function DirectoryDescriptor d -> d.ParentDirectoryName = parentName | FileDescriptor f -> f.ParentDirectoryName = parentName)
//
// let nestedFileDefinitions =
//     fileDefinitions
//     |> List.filter (function DirectoryDescriptor _ -> true | _ -> false)
//     |> List.map (function DirectoryDescriptor d -> DirectoryDescriptor {d with FileDefinitions = (withParents (Some d.Name))} | FileDescriptor f -> FileDescriptor f)
//
// let rec calculateDirectorySize (directory:DirectoryDescriptor) =
//     let size =
//         directory.FileDefinitions
//         |> List.map (function DirectoryDescriptor d -> calculateDirectorySize d | FileDescriptor f -> f.Size)
//         |> List.sum
//     fileDefinitions
//     |> List.map (function DirectoryDescriptor d -> if d.Name =         | FileDescriptor f -> f.Size)
//
// let part1 =
//     nestedFileDefinitions
//     |> List.filter (function DirectoryDescriptor d -> d.Name = "/" | _ -> false)
//     |> List.head
//     
