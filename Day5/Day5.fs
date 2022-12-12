module AdventCalendar2022.Day5

open System.Text.RegularExpressions

type Move = { Count: int; From: int; To: int }

let moves =
    System.IO.File.ReadAllLines("Day5/input.txt")
    |> Array.map (fun line ->
        let regexMatch =
            Regex.Matches(line, "move ([0-9]+) from ([0-9]+) to ([0-9]+)")
            |> Seq.head

        { Count = int (regexMatch.Groups[ 1 ].ToString())
          From = (int (regexMatch.Groups[ 2 ].ToString())) - 1
          To = (int (regexMatch.Groups[ 3 ].ToString())) - 1 })
    |> Array.toList

let stacks =
    [
      [ 'T';'R';'D';'H';'Q';'N';'P';'B' ]
      [ 'V'; 'T'; 'J'; 'B'; 'G'; 'W' ]
      [ 'Q';'M';'V';'S';'D';'H';'R';'N' ]
      [ 'C'; 'M'; 'N'; 'Z'; 'P' ]
      [ 'B'; 'Z'; 'D' ]
      [ 'Z'; 'W'; 'C'; 'V' ]
      [ 'S';'L';'Q';'V';'C';'N';'Z';'G' ]
      [ 'V'; 'N'; 'D'; 'M'; 'J'; 'G'; 'L' ]
      [ 'G'; 'C'; 'Z'; 'F'; 'M'; 'P'; 'T' ]
    ]

let testStacks =
    [
      [ 'N'; 'Z' ]
      [ 'D'; 'C'; 'M' ]
      [ 'P' ]
    ]

let updateStacks (fromIdx:int) (toIdx:int) fromStack toStack stacks =
    stacks
    |> List.mapi (
                  fun i x ->
                  if i = fromIdx then fromStack
                  else
                  if i = toIdx then toStack
                  else
                  x
                )

let rec parseMoves (accStacks: char list list) moves =
    match moves with
    | currentMove :: nextMoves ->
        let stackFrom = accStacks[currentMove.From]
        let stackTo = accStacks[currentMove.To]
        
        let crates =
            stackFrom
            |> List.take currentMove.Count
            |> List.rev

        let newStackFrom =
            stackFrom |> List.skip currentMove.Count

        let newStackTo =
            stackTo |> List.append crates
        
        parseMoves (updateStacks currentMove.From currentMove.To newStackFrom newStackTo accStacks) nextMoves
    | [] -> accStacks

let topOfStacks stacks =
    stacks
    |> List.map (fun x -> x |> List.head)
        
let part1 = moves
            |> parseMoves stacks
            |> topOfStacks
            |> List.map (fun x -> x.ToString())
            |> String.concat ""
            
let rec parseMoves9001 (accStacks: char list list) moves =
    match moves with
    | currentMove :: nextMoves ->
        let stackFrom = accStacks[currentMove.From]
        let stackTo = accStacks[currentMove.To]
        
        let crates =
            stackFrom
            |> List.take currentMove.Count

        let newStackFrom =
            stackFrom |> List.skip currentMove.Count

        let newStackTo =
            stackTo |> List.append crates
        
        parseMoves9001 (updateStacks currentMove.From currentMove.To newStackFrom newStackTo accStacks) nextMoves
    | [] -> accStacks

let part2 = moves
            |> parseMoves9001 stacks
            |> topOfStacks
            |> List.map (fun x -> x.ToString())
            |> String.concat ""