module AdventCalendar2022.Day8

let inline charToInt c = int c - int '0'

type Tree = {
    Row: int
    Col: int
    Height: int
    mutable Visible: bool
}

let rawInput =
    System.IO.File.ReadAllLines("Day8/input.txt")

let groveHeight = rawInput.Length
let groveWidth = rawInput[0].Length

let grove = rawInput
            |> Array.mapi (fun r line -> line.ToCharArray() |> Array.mapi (fun c char -> {Row=r; Col=c; Height=charToInt char; Visible= r = 0 || c = 0 || r = groveHeight - 1 || c = groveWidth - 1}))
   
let mutTopDownScan =
    [|1..groveWidth-2|]
    |> Array.map (
        fun col ->
            let mutable currentHighestTree = (grove[0][col]).Height
            for row in [|1..groveHeight - 2|] do
                if (grove[row][col]).Height > currentHighestTree
                then
                    (grove[row][col]).Visible <- true
                    currentHighestTree <- (grove[row][col]).Height            
            )
let mutBottomUpScan =
    [|1..groveWidth-2|]
    |> Array.map (
        fun col ->
            let mutable currentHighestTree = (grove[groveHeight-1][col]).Height
            for row in ([|1..groveHeight - 2|] |> Array.rev) do
                if (grove[row][col]).Height > currentHighestTree
                then
                    (grove[row][col]).Visible <- true
                    currentHighestTree <- (grove[row][col]).Height            
            ) 

let mutLeftRightScan =
    [|1..groveHeight-2|]
    |> Array.map (
        fun row ->
            let mutable currentHighestTree = (grove[row][0]).Height
            for col in [|1..groveWidth - 2|] do
                if (grove[row][col]).Height > currentHighestTree
                then
                    (grove[row][col]).Visible <- true
                    currentHighestTree <- (grove[row][col]).Height            
            )
    
let mutRightLeftScan =
    [|1..groveHeight-2|]
    |> Array.map (
        fun row ->
            let mutable currentHighestTree = (grove[row][groveWidth - 1]).Height
            for col in ([|1..groveWidth - 2|] |> Array.rev) do
                if (grove[row][col]).Height > currentHighestTree
                then
                    (grove[row][col]).Visible <- true
                    currentHighestTree <- (grove[row][col]).Height            
            )

let part1 = grove
            |> Array.map (fun row -> (row |> Array.filter (fun c -> c.Visible)).Length)
            |> Array.sum
            

let rec countViewUp row col myHeight =
    if row = 0
    then 1
    else        
    if (grove[row][col]).Height < myHeight
    then
        (countViewUp (row-1) col myHeight) + 1
    else
        1
    
let scanUp tree =
    if tree.Row = 0
    then 0
    else
        countViewUp (tree.Row-1) tree.Col tree.Height
        
let rec countViewDown row col myHeight =
    if row = groveHeight - 1
    then 1
    else        
    if (grove[row][col]).Height < myHeight
    then
        (countViewDown (row+1) col myHeight) + 1
    else
        1
let scanDown tree =
    if tree.Row = groveHeight - 1
    then 0
    else
        countViewDown (tree.Row+1) tree.Col tree.Height
        
let rec countViewLeft row col myHeight =
    if col = 0
    then 1
    else        
    if (grove[row][col]).Height < myHeight
    then
        (countViewLeft row (col-1) myHeight) + 1
    else
        1
        
let scanLeft tree =
    if tree.Col = 0
    then 0
    else
        countViewLeft tree.Row (tree.Col-1) tree.Height
        
let rec countViewRight row col myHeight =
    if col = groveWidth - 1
    then 1
    else        
    if (grove[row][col]).Height < myHeight
    then
        (countViewRight row (col+1) myHeight) + 1
    else
        1
let scanRight tree =
    if tree.Col = groveWidth - 1
    then 0
    else
        countViewRight tree.Row (tree.Col+1) tree.Height
    
let scenicScore tree =
    scanUp tree *
    scanDown tree *
    scanLeft tree *
    scanRight tree
    
let part2 = grove
            |> Array.map (
                fun row ->
                    row
                    |> Array.map scenicScore
                    |> Array.max
                )
            |> Array.max