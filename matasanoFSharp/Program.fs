// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open MainMonad
open One

[<EntryPoint>]
let main argv = 
    let input = Console.ReadLine()
    let output = (One.hexStringToByteList input)
    match output with
    | OK a -> a |> List.iter (fun a -> Console.WriteLine (Convert.ToString(a,2).PadLeft(8,'0')))
    | Error message -> printfn "Error message: %s" message
    Console.ReadLine() |> ignore
    0 // return an integer exit code
