module QTests
open FsUnit
open NUnit.Framework
open MM

[<Test>]
let S1Q1Tests() =
    "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
        |> QS.s1q1
        |> should equal (OK "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t")
[<Test>]
let S1Q1EasterEgg() =
    "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
        |> QS.s1q1ee
        |> should equal (OK "I'm killing your brain like a poisonous mushroom")

[<Test>]
let S1Q2Tests() =
    ("1c0111001f010100061a024b53535009181c","686974207468652062756c6c277320657965")
        ||> QS.s1q2
        |> should equal (OK ("746865206b696420646f6e277420706c6179".ToUpper()))
[<Test>]
let S1Q2EasterEgg() =
    [|
        "686974207468652062756c6c277320657965"
        "746865206b696420646f6e277420706c6179"
    |]
    |> Array.map QS.s1q1ee
    |> should equal 
        ([|
            OK "hit the bull's eye"
            OK "the kid don't play"
        |])

[<Test>]
let s1q3test() =
    QS.s1q3 "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"
    |> should equal (OK "Cooking MC's like a pound of bacon")

[<Test>]
let s1q4test() =
    QS.s1q4()
    |> should equal (OK "Now that the party is jumping\n")