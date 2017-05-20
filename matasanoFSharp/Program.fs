// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open MM
open One

//let Set1Challenge1 = One.hexStringToBase64String("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d")
//let Set1Challenge2 = One.hexStringXOR "1c0111001f010100061a024b53535009181c" "686974207468652062756c6c277320657965"

[<EntryPoint>]
let main argv = 
//    StringAnalysis.testSingleXOR
    IN.tests()
    Console.ReadLine() |> ignore
    0 // return an integer exit code
