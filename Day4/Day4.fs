module AdventCalendar2022.Day4

let lineParse (line:string) =
    let splitLine = line.Split(",")
    let part1Split = splitLine[0].Split("-")
    let part2Split = splitLine[1].Split("-")
    ((int part1Split[0], int part1Split[1]), (int part2Split[0], int part2Split[1]))
  
let input =
    System.IO.File.ReadAllLines("Day4/input.txt")
    |> Array.map lineParse
    
let fullyContains (part1, part2) =
    fst part1 <= fst part2 && snd part1 >= snd part2
    || fst part2 <= fst part1 && snd part2 >= snd part1
    
let part1 =
    input
    |> Array.filter fullyContains
    |> Array.length

let overlaps (part1, part2) =
    fst part1 <= fst part2 && snd part1 >= fst part2
    || fst part1 <= snd part2 && snd part1 >= snd part2    
    || fst part2 <= fst part1 && snd part2 >= fst part1
    || fst part2 <= snd part1 && snd part2 >= snd part1
    
let part2 =
    input
    |> Array.filter overlaps
    |> Array.length
    
    
    

    
// ******************************
// read the part2 question wrong
let flattenedInput =
    input
    |> Array.collect (fun (p1, p2) -> [|p1;p2|])
    |> Array.toList
    
let rec countFullyContains ranges =
    match ranges with
    | head::tail ->
        (
         tail
            |> List.filter (fun range -> fullyContains (head, range))
            |> List.length
        )
        +
        (countFullyContains tail)
    | [] -> 0
// ******************************