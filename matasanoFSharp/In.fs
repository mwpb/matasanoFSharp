module IN
open System
open System.Diagnostics
open System.Collections
open MM

let charToByte (c:char) :Error<byte>= 
    try
        OK (System.Convert.ToByte(c))
    with 
    | _ -> Error (sprintf "Cannot convert char (%c) to a byte" c)
    
let hexPairToByte (c1:char) (c2:char) :Error<byte>=
    try
        OK (System.Convert.ToByte(string(c1)+string(c2),16))
    with
    | _ -> Error (sprintf "Cannot convert pair (%c,%c) to byte" c1 c2)

let base64QuadrupleToByteTriple (c1:char) (c2:char) (c3:char) (c4:char) :Error<byte*byte*byte>=
    let byteArray = System.Convert.FromBase64CharArray([|c1;c2;c3;c4|],0,4)
    OK (byteArray.[0],byteArray.[1],byteArray.[2])