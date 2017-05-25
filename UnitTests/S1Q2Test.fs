module S1Q2Test
open MM
open NUnit.Framework
open FsUnit

[<Test>]
let hexStringXORTests() =
    ("1c0111001f010100061a024b53535009181c","686974207468652062756c6c277320657965")
        ||> S1Q2.hexStringXOR
        |> should equal (OK ("746865206b696420646f6e277420706c6179".ToUpper()))
[<Test>]
let XOREasterEgg() =
    [|
        "686974207468652062756c6c277320657965"
        "746865206b696420646f6e277420706c6179"
    |]
    |> Array.map S1Q1.hexStringToCharString
    |> should equal 
        ([|
            OK "hit the bull's eye"
            OK "the kid don't play"
        |])