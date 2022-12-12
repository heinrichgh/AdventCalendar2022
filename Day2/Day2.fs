module AdventCalendar2022.Day2

open System    
let input =
    System.IO.File.ReadAllLines("Day2/input.txt")
    |> Array.map (fun line -> (line.Split(' ')[1], line.Split(' ')[0]))

let winScore = 6
let drawScore = 3
let loseScore = 0
let rockScore = 1
let paperScore = 2
let scissorsScore = 3

type Shape = Rock | Paper | Scissors
let decode a =
    match a with
    | "A" -> Rock
    | "B" -> Paper
    | "C" -> Scissors
    | "X" -> Rock
    | "Y" -> Paper
    | "Z" -> Scissors
    | _ ->  raise <| new Exception("Invalid encoded symbol for Rock/Paper/Scissors")
    
let checkMatchResult played =
    match played with 
    | (Rock, Rock) -> drawScore
    | (Rock, Paper) -> loseScore
    | (Rock, Scissors) -> winScore
    | (Paper, Rock) -> winScore
    | (Paper, Paper) -> drawScore
    | (Paper, Scissors) -> loseScore
    | (Scissors, Rock) -> loseScore
    | (Scissors, Paper) -> winScore
    | (Scissors, Scissors) -> drawScore
    
let checkShapeResult played =
    match played with
    | (Rock, _) -> rockScore
    | (Paper, _) -> paperScore
    | (Scissors, _) -> scissorsScore

let play played = checkMatchResult played + checkShapeResult played 

let part1 =
       input
       |> Array.map ((fun (x,y) -> (decode x, decode y)) >> play)
       |> Array.sum
       
type outcome = Win | Draw | Lose
 
let decodeOutcome outcome =
    match outcome with
    | "X" -> Lose
    | "Y" -> Draw
    | "Z" -> Win
    | _ -> raise <| new Exception("Invalid encoded symbol for outcome")
      
let shouldPlay strategy =
    match strategy with
    | (Lose, Rock) -> Scissors
    | (Lose, Paper) -> Rock
    | (Lose, Scissors) -> Paper
    | (Draw, a) -> a
    | (Win, Rock) -> Paper
    | (Win, Paper) -> Scissors
    | (Win, Scissors) -> Rock

let part2 =
       input
       |> Array.map (
           (fun (x,y) -> (decodeOutcome x, decode y))           
           >> (fun (outcome, opponentPlayed) -> (shouldPlay (outcome, opponentPlayed), opponentPlayed))
           >> play
           )
       |> Array.sum