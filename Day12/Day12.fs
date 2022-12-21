module AdventCalendar2022.Day12

open System

type Vertex =
    {
        Position: int * int
        Distance: int
        Visited: bool
        Value: int
     }

let mutable endPos = (-1, -1)

let vertices =
    System.IO.File.ReadAllLines("Day12/input.txt")
    |> Array.mapi (fun rowI line ->

        line.ToCharArray()
        |> Array.mapi (fun colI char ->
            if char = 'S' then
                {
                    Position= (rowI, colI)
                    Distance= 0
                    Visited= false
                    Value= int 'a'
                }
            else if char = 'E' then
                endPos <- (rowI, colI)
                {
                    Position= (rowI, colI)
                    Distance= Int32.MaxValue
                    Visited= false
                    Value= int 'z'
                }
            else
                {
                    Position= (rowI, colI)
                    Distance= Int32.MaxValue
                    Visited= false
                    Value= int char
                }))
    |> Array.collect id
    |> Array.map (fun v -> (v.Position, v))
    |> Map.ofArray


// Adjacency list
// i = (rowI * colW) + colI

let topPosition vertex = (fst vertex.Position - 1, snd vertex.Position)
let rightPosition vertex = (fst vertex.Position, snd vertex.Position + 1)
let bottomPosition vertex = (fst vertex.Position + 1, snd vertex.Position)
let leftPosition vertex = (fst vertex.Position, snd vertex.Position - 1)

let getNeighbours vertex unVisitedList =
    [topPosition vertex; rightPosition vertex; bottomPosition vertex; leftPosition vertex]
    |> List.map (fun pos ->
        if unVisitedList |> Map.containsKey pos
        then
            let vert = unVisitedList |> Map.find pos
            if vert.Value - vertex.Value <= 1
            then Some vert
            else None
        else
            None)
   |> List.choose id
    
let getShortestFromUnvisited unVisitedList =
    let sortedList =
        unVisitedList
        |>  Map.toArray
        |> Array.map snd
        |> Array.sortBy (fun v -> v.Distance)
    
    let shortest = sortedList |> Array.head
    shortest, unVisitedList |> Map.remove shortest.Position
    
let updateVertexDistanceInMap distanceToAdd vertex =
    match vertex with
    | Some v -> Some {v with Distance=1 + distanceToAdd}
    | None -> None
    
let rec updateNeighbours vertex unVisitedList neighbours =
    match neighbours with
    | head::tail ->
        let distanceAdder = updateVertexDistanceInMap vertex.Distance
        updateNeighbours vertex (unVisitedList |> Map.change head.Position distanceAdder) tail
    | [] -> unVisitedList

let rec findPathLength vertex unVisitedList =
    if vertex.Position = endPos
    then vertex.Distance
    else
        let neighbours = getNeighbours vertex unVisitedList
        let updatedUnVisitedList = updateNeighbours vertex unVisitedList neighbours
        let (nextVertex, newUnVisitedList) = getShortestFromUnvisited updatedUnVisitedList
        findPathLength nextVertex  newUnVisitedList
        
// Get neighbours

let (nextVertex, newUnVisitedList) = getShortestFromUnvisited vertices

let part1 = findPathLength nextVertex  newUnVisitedList
