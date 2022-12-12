module AdventCalendar2022.Day10

let parseLine (line: string) =
    if line.Substring(0, 4) = "noop"
    then None
    else
        Some (int (line.Split(" ")[1]))      

let instructions =
    System.IO.File.ReadAllLines("Day10/input.txt")
    |> Array.map parseLine
    |> Array.toList
    
let rec parseInstructions acc instructions =
    let currentRegisterValue = (acc |> List.head)
    match instructions with
    | current::rest ->
        match current with
        | Some size ->
            parseInstructions ((size+currentRegisterValue)::currentRegisterValue::acc) rest
        | None -> parseInstructions (currentRegisterValue::acc) rest
    | [] -> acc |> List.rev
    
let cycles = parseInstructions [1] instructions

let signalStrength time = time * cycles[time-1] 
let part1 = signalStrength 20 +
            signalStrength 60 +
            signalStrength 100 +
            signalStrength 140 +
            signalStrength 180 +
            signalStrength 220     

let drawCycles =
    [0..239]
    |> List.map (fun cycle ->
                    let register = cycles[cycle]
                    let pixel = cycle % 40
                    if register-1 = pixel || register = pixel || register+1 = pixel
                    then "#"
                    else "."        
        )
    
let printCrtLine cycleStart cycleEnd =
    let line = (drawCycles[cycleStart-1..cycleEnd-1] |> String.concat "")
    printfn $"{line}"
    
let part2 =     
    printCrtLine 1 40
    printCrtLine 41 80
    printCrtLine 81 120
    printCrtLine 121 160
    printCrtLine 161 200
    printCrtLine 201 240