// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Diagnostics
open One

//let Set1Challenge1 = One.hexStringToBase64String("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d")
//let Set1Challenge2 = One.hexStringXOR "1c0111001f010100061a024b53535009181c" "686974207468652062756c6c277320657965"

[<EntryPoint>]
let main argv = 
//    StringAnalysis.testSingleXOR
//    Debug.WriteLine (sprintf "%A" IN.testAsciiToByteFailure)
    let out =  QS.s1q8()
//    for x in out do Console.Write (sprintf "%A" x)
    //MM.printError (out)
    Console.WriteLine out
    Console.ReadLine() |> ignore
    0 // return an integer exit code 