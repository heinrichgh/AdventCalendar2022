module AdventCalendar2022.Day1

let input =
    System.IO.File.ReadAllText("Day1/input.txt")

let newLine = "\r\n"
let part1 =
       input.Split(newLine + newLine)
       |> Array.map (fun x -> x.Split(newLine) |> Array.map int |> Array.sum)
       |> Array.max
               
let part2 =
       input.Split(newLine + newLine)
       |> Array.map (fun x -> x.Split(newLine) |> Array.map int |> Array.sum)
       |> Array.sortDescending
       |> Array.take 3
       |> Array.sum