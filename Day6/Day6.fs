module AdventCalendar2022.Day6

let datastreamBuffer =
    System.IO.File.ReadAllText("Day6/input.txt").ToCharArray()
    |> Array.toList
    
let rec markerPos counter bufferLeft =
    let b1::b2::b3::b4::tail = bufferLeft
    if
        b1 <> b2
        && b1 <> b3
        && b1 <> b4
        && b2 <> b3
        && b2 <> b4
        && b3 <> b4
    then
        counter + 4
    else
        markerPos (counter+1) (bufferLeft |> List.skip 1)
        
let part1 =
    datastreamBuffer
    |> markerPos 0
    
let checkDistinct list =
    let distinctList = list |> List.distinct
    list.Length = distinctList.Length

let rec markerPosP2 counter markerLength bufferLeft =    
    if checkDistinct (bufferLeft |> List.take markerLength)
    then
        counter + markerLength
    else
        markerPosP2 (counter+1) markerLength (bufferLeft |> List.skip 1)
      
let part2 =
    datastreamBuffer
    |> markerPosP2 0 14