module AdventCalendar2022.Day3

open System
open System.Text.RegularExpressions    
let input =
    System.IO.File.ReadAllLines("Day3/input.txt")
    |> Array.map (fun line -> line.ToCharArray())
    
let typePriority x =
    match x with
    | _ when Regex.Match(string x, "[a-z]").Success -> (int x) - 96
    | _ when Regex.Match(string x, "[A-Z]").Success -> (int x) - 38
    | _ -> raise <| new Exception("Invalid type packed in bag")

let findWronglyPlaced (compartment1 : char[], compartment2) =
    Array.concat [
        (
            compartment1
            |> Array.filter (fun item -> compartment2 |> Array.exists (fun i -> i = item) )
        );
        (
            compartment2
            |> Array.filter (fun item -> compartment1 |> Array.exists (fun i -> i = item) )
        )
    ]
    |> Array.head
    
let part1 = input
            |> Array.map (
                   (fun bag -> (Array.take (bag.Length/2) bag, bag |> Array.skip (bag.Length/2) |> Array.take (bag.Length/2)))
                   >> findWronglyPlaced
                   >> typePriority
                 )
            |> Array.sum
        
let findMatchingType (bag1 : char[]) bag2 bag3 =
    let set1 = bag1
               |> Array.filter (fun item -> bag2 |> Array.exists (fun i -> i = item) )
               |> Set.ofArray
    let set2 = bag1
               |> Array.filter (fun item -> bag3 |> Array.exists (fun i -> i = item) )
               |> Set.ofArray
    Set.intersect set1 set2 |> Set.toArray |> Array.head                     
                   
let rec searchBags acc bags =
        match bags with
        | bag1::bag2::bag3::tail -> searchBags ((findMatchingType bag1 bag2 bag3)::acc) tail
        | [] -> acc
        | _ -> raise <| new Exception("bags must come in threes")
        
let part2 = input
            |> Array.toList
            |> searchBags []
            |> List.map typePriority
            |> List.sum