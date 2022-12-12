module AdventCalendar2022.Day11

open System.Collections.Generic
open System.Text.RegularExpressions

type MonkeyDefinition =
    {
        Items: int list
        Worry: int -> int
        NextMonkey: int -> int
    }
    
let parseOperation (line:string) =
    let operation = line.Substring(line.IndexOf("old")+4)
    if operation[0] = '*'
    then
        if operation.Contains "old"
        then
            (fun x -> x * x)
        else              
            let number = int (Regex.Match(line, "([0-9]+)").Groups[1].ToString())
            (fun x -> x * number)            
    else
       if operation.Contains "old"
        then
            (fun x -> x + x)
        else              
            let number = int (Regex.Match(line, "([0-9]+)").Groups[1].ToString())
            (fun x -> x + number)     

let mutable modNumber = 1

let parseMonkey (text : string) =
    let lines = text.Split("\r\n")
    let startingItems = lines[1].Substring(lines[1].IndexOf(":")+1).Split(",") |> Array.map int |> Array.toList
    let operation = parseOperation lines[2]
    let divisible = int (Regex.Match(lines[3], "([0-9]+)").Groups[1].ToString())
    let trueMonkey = int (Regex.Match(lines[4], "([0-9]+)").Groups[1].ToString())
    let falseMonkey = int (Regex.Match(lines[5], "([0-9]+)").Groups[1].ToString())
        
    modNumber <- modNumber * divisible
    {
        Items=startingItems
        Worry=operation
        NextMonkey=(fun worry -> if worry % divisible = 0 then trueMonkey else falseMonkey)
    }
    

let monkeys =
    System.IO.File.ReadAllText("Day11/test.txt").Split("\r\n\r\n")
    |> Array.map parseMonkey


let monkeyThrows monkey =
    monkey.Items
    |> List.map (fun item ->
            let worry = int (floor (float (monkey.Worry(item)) / 3.0))
            (monkey.NextMonkey(worry), worry)            
        )
    |> List.groupBy (fun (monkey, _) -> monkey)
    |> List.map (fun (key, values) -> (key, values |> List.map snd))
    |> dict
    
let countThrows (throws: IDictionary<int, int list>) =
    throws.Keys
    |> Seq.map (fun key -> throws[key].Length)
    |> Seq.sum
    

let rec playRound monkeyNumber (monkeys: MonkeyDefinition[]) inspectionCount =
    let throws = monkeyThrows monkeys[monkeyNumber]
    
    let updInspectionCount = inspectionCount
                             |> List.mapi (fun i v -> if i = monkeyNumber then v + countThrows throws else v)
    
    let updMonkeys = monkeys
                     |> Array.mapi (fun i monkey ->
                         if i = monkeyNumber
                         then {monkey with Items=[]}
                         else    
                         if throws.ContainsKey(i)
                         then {monkey with Items=monkey.Items @ throws[i]}
                         else monkey                             
                             )
    if monkeyNumber = (monkeys |> Array.length) - 1
    then (updMonkeys, updInspectionCount)
    else playRound (monkeyNumber+1) updMonkeys updInspectionCount


let rec playMultipleRounds roundCount (monkeys, inspectionCount) =
    if roundCount = 0
    then (monkeys, inspectionCount)
    else
        playMultipleRounds (roundCount-1) (playRound 0 monkeys inspectionCount)

let part1 = playMultipleRounds 20 (monkeys, [for _ in 1..monkeys.Length -> 0])
            |> snd
            |> List.sortDescending
            |> List.take 2
            |> List.fold (fun acc v -> acc * v) 1

let monkeyThrows2 monkey =
    monkey.Items
    |> List.map (fun item ->
            let worry = monkey.Worry(item) % modNumber
            (monkey.NextMonkey(worry), worry)            
        )
    |> List.groupBy (fun (monkey, _) -> monkey)
    |> List.map (fun (key, values) -> (key, values |> List.map snd))
    |> dict
        

let rec playRound2 monkeyNumber (monkeys: MonkeyDefinition[]) inspectionCount =
    let throws = monkeyThrows2 monkeys[monkeyNumber]
    
    let updInspectionCount = inspectionCount
                             |> List.mapi (fun i v -> if i = monkeyNumber then v + countThrows throws else v)
    
    let updMonkeys = monkeys
                     |> Array.mapi (fun i monkey ->
                         if i = monkeyNumber
                         then {monkey with Items=[]}
                         else    
                         if throws.ContainsKey(i)
                         then {monkey with Items=monkey.Items @ throws[i]}
                         else monkey                             
                             )
    if monkeyNumber = (monkeys |> Array.length) - 1
    then (updMonkeys, updInspectionCount)
    else playRound2 (monkeyNumber+1) updMonkeys updInspectionCount


let rec playMultipleRounds2 roundCount (monkeys, inspectionCount) =
    if roundCount = 0
    then (monkeys, inspectionCount)
    else
        playMultipleRounds2 (roundCount-1) (playRound2 0 monkeys inspectionCount)

let part2 = playMultipleRounds2 20 (monkeys, [for _ in 1..monkeys.Length -> 0])
            |> snd
            // |> List.sortDescending
            // |> List.take 2
            // |> List.map bigint
            // |> List.fold (fun acc v -> acc * v) (bigint 1)