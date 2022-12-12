module AdventCalendar2022.Day9

open System

type Direction =
    | Up
    | Down
    | Left
    | Right

type Motion = { Direction: Direction; Steps: int }

type Coord = {X:int; Y:int}

let motions =
    System.IO.File.ReadAllLines("Day9/input.txt")
    |> Array.map (fun line ->
        let splits = line.Split(" ")

        { Direction =
            match splits[0][0] with
            | 'U' -> Up
            | 'D' -> Down
            | 'L' -> Left
            | 'R' -> Right
            | _ ->
                raise
                <| Exception("Invalid Direction Character")

          Steps = int splits[1] })
    |> Array.toList
    
let isTouching headPos tailPos = abs (headPos.X - tailPos.X) <= 1 && abs (headPos.Y - tailPos.Y) <= 1
let sameVertical headPos tailPos = headPos.X - tailPos.X = 0
let sameHorizontal headPos tailPos = headPos.Y - tailPos.Y = 0
let headAboveTail headPos tailPos = headPos.Y > tailPos.Y
let headLeftOfTail headPos tailPos = headPos.X < tailPos.X
    
let tailMove headPos tailPos =
    if isTouching headPos tailPos
    then tailPos
    else
    if sameVertical headPos tailPos
    then
        if headAboveTail headPos tailPos
        then {tailPos with Y=tailPos.Y+1}
        else {tailPos with Y=tailPos.Y-1}
    else
    if sameHorizontal headPos tailPos
    then
        if headLeftOfTail headPos tailPos
        then {tailPos with X=tailPos.X-1}
        else {tailPos with X=tailPos.X+1}
    else
        {
            tailPos with
                X = if headLeftOfTail headPos tailPos then tailPos.X-1 else tailPos.X+1
                Y = if headAboveTail headPos tailPos then tailPos.Y+1 else tailPos.Y-1
        }
        
let rec moveUp headPos tailPos steps =
    if steps = 0
    then []
    else
    let newHeadPos = {headPos with Y=headPos.Y+1}
    let newTailPos = tailMove newHeadPos tailPos
    newTailPos::(moveUp newHeadPos newTailPos (steps-1))
    
let rec moveDown headPos tailPos steps =
    if steps = 0
    then []
    else
    let newHeadPos = {headPos with Y=headPos.Y-1}
    let newTailPos = tailMove newHeadPos tailPos
    newTailPos::(moveDown newHeadPos newTailPos (steps-1))
    
let rec moveLeft headPos tailPos steps =
    if steps = 0
    then []
    else
    let newHeadPos = {headPos with X=headPos.X-1}
    let newTailPos = tailMove newHeadPos tailPos
    newTailPos::(moveLeft newHeadPos newTailPos (steps-1))
    
let rec moveRight headPos tailPos steps =
    if steps = 0
    then []
    else
    let newHeadPos = {headPos with X=headPos.X+1}
    let newTailPos = tailMove newHeadPos tailPos
    newTailPos::(moveRight newHeadPos newTailPos (steps-1))

let rec simulate (motions: Motion list) headPos tailPos tailVisits =
    match motions with
    | motion::futureMotions ->
        let moved = 
            match motion.Direction with
            | Up ->
                let tailPositions = moveUp headPos tailPos motion.Steps // |> List.distinct
                ({headPos with Y=headPos.Y+motion.Steps}, tailPositions)
            | Down -> 
                let tailPositions = moveDown headPos tailPos motion.Steps // |> List.distinct
                ({headPos with Y=headPos.Y-motion.Steps}, tailPositions)
            | Left -> 
                let tailPositions = moveLeft headPos tailPos motion.Steps // |> List.distinct
                ({headPos with X=headPos.X-motion.Steps}, tailPositions)
            | Right -> 
                let tailPositions = moveRight headPos tailPos motion.Steps // |> List.distinct
                ({headPos with X=headPos.X+motion.Steps}, tailPositions)        
        simulate futureMotions (fst moved) ((snd moved) |> List.last) (tailVisits @ (snd moved) )
    | [] -> tailVisits

let part1 = simulate motions {X=0;Y=0} {X=0;Y=0} []
            |> List.distinct
            |> List.length
            
let rec moveKnots head knots =
    match knots with
    | moving::tail ->
        let moved = tailMove head moving
        moved::(moveKnots moved tail)
    | [] -> []
    
let rec move headPos (knots: Coord list) (tailPositions: Coord list) steps direction =
    if steps = 0
    then (headPos, knots, tailPositions)
    else
    let newHeadPos = direction headPos
    let newKnots = moveKnots newHeadPos knots
    move newHeadPos newKnots ((newKnots |> List.last)::tailPositions) (steps-1) direction
            
let rec simulate2 (motions: Motion list) head (knots: Coord list) tailVisits =
    match motions with
    | motion::futureMotions ->
        let movement = move head knots [] motion.Steps
        let (headPos, knots, tailPositions) =
            match motion.Direction with
            | Up ->
                movement (fun headPos -> {headPos with Y=headPos.Y+1}) 
            | Down -> 
                movement (fun headPos -> {headPos with Y=headPos.Y-1})
            | Left -> 
                movement (fun headPos -> {headPos with X=headPos.X-1})
            | Right -> 
                movement (fun headPos -> {headPos with X=headPos.X+1})     
        simulate2 futureMotions headPos knots (tailVisits @ tailPositions)
    | [] -> tailVisits
    
let part2 = simulate2 motions {X=0;Y=0} [for _ in 1..9 -> {X=0;Y=0}] []
            |> List.distinct
            |> List.length