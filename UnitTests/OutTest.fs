module OutTest

open FsUnit
open NUnit.Framework
open MM

[<Test>]
let testByteToHexPairSuccess() =
    [|
        0b0uy , '0', '0'
        0b1uy , '0', '1'
        0b10uy, '0', '2'
        0b11uy, '0', '3'
    |] |> Array.iter (fun (b,x,y) -> OUT.byteToHexPair b |> should equal (OK (x,y)) )