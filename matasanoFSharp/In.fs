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

let rec split2 (array:'a []) :Error<('a*'a)[]>=
    er {
        if array.Length = 0 then return [||]
        elif (array.Length < 2) then return! Error (sprintf "Length not divisble by 2: %A" array)
        else
            let! recursion = split2 array.[2..] 
            return Array.concat([[|(array.[0],array.[1])|];recursion])
    }

let rec split3 (array:'a []) :Error<('a*'a*'a)[]>=
    er {
        if array.Length = 0 then return [||]
        elif (array.Length < 2) then return! Error (sprintf "Length not divisble by 3: %A" array)
        else
            let! recursion = split3 array.[3..] 
            return Array.concat([[|(array.[0],array.[1],array.[2])|];recursion])
    }

let rec split4 (array:'a []) :Error<('a*'a*'a*'a)[]>=
    er {
        if array.Length = 0 then return [||]
        elif (array.Length < 2) then return! Error (sprintf "Length not divisble by 3: %A" array)
        else
            let! recursion = split4 array.[4..] 
            return Array.concat([[|(array.[0],array.[1],array.[2],array.[3])|];recursion])
    }
