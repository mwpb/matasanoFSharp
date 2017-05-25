module S1Q1Test
open FsUnit
open NUnit.Framework
open MM

[<Test>]
let hexStringToBase64StringTests() =
    "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
        |> S1Q1.hexStringToBase64String
        |> should equal (OK "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t")
[<Test>]
let hexStringToCharStringTests() =
    "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
        |> S1Q1.hexStringToCharString
        |> should equal (OK "I'm killing your brain like a poisonous mushroom")
