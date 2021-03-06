﻿module ByteOps
open AU
open System.Diagnostics

let byteXOR (b1:byte) (b2:byte) = [|b1^^^b2|]
let rec bytesXORInner (acc:byte []) (bytes1:byte []) (bytes2: byte []) =
    match bytes1, bytes2 with
    | Nil, Nil -> acc
    | Nil, Cons(a,tail) | Cons(a,tail),Nil -> failwith "Bytes arrays should have the same length."
    | Cons(a,taila), Cons(b,tailb) -> bytesXORInner (Array.append acc (byteXOR a b)) taila tailb
let bytesXOR (bytes1:byte []) (bytes2: byte []) = bytesXORInner Array.empty bytes1 bytes2

let rec bytesXORSingleInner (acc:byte []) (bytes1:byte []) (b:byte) =
    match bytes1 with
    | Nil -> acc
    | Cons(a,tail) -> bytesXORSingleInner (Array.append acc (byteXOR a b)) tail b
let bytesXORSingle (bytes1:byte []) (b:byte) = bytesXORSingleInner Array.empty bytes1 b

let rec transposeInner (acc:byte [] []) (keyLength:int) (currentKeyIndex:int) (bytes:byte []) =
//    Debug.WriteLine (sprintf "%A" bytes)
    match bytes, currentKeyIndex with
    | Nil, _ -> acc
    | Cons(a,tail), k -> 
        let j = k%keyLength
        acc.[j] <- (Array.append (acc.[j]) [|a|])
        transposeInner acc keyLength (j+1%keyLength) tail
let transpose (keyLength:int) (bytes:byte []) = transposeInner [|for i in [0..keyLength-1] do yield [||]|] keyLength 0 bytes

//
//let rec transpose (n:int) (bytes:byte []) =
//    let m = bytes.Length
//    Debug.WriteLine (sprintf "m=%d, n=%d" m n)
//    [|for j in 0..(n-1) do yield [|for i in [|j..n..m|] do yield bytes.[i]|]|]

let rec revtransposeInner (acc:byte []) (keyLength:int) (currentKeyIndex:int) (transpose: byte [] []) =
//    Debug.WriteLine (sprintf "%A" transpose)
//    Debug.WriteLine (sprintf "%A" transpose)
    match transpose.[currentKeyIndex] with
    | Nil -> acc
    | Cons(a,tail) -> 
        let j = (currentKeyIndex)%keyLength
        transpose.[j] <- tail
        revtransposeInner (Array.append acc [|a|]) keyLength ((j+1)%keyLength) transpose
let revtranspose (keyLength:int) (transpose: byte [] []) = revtransposeInner Array.empty keyLength 0 transpose

//let rec revtranspose (transpose: byte [] []) =
//    let row j = [|for i in 0..transpose.Length-1 do if j<(transpose.[i].Length-1) then yield transpose.[i].[j]|]
//    Array.concat [for j in 0..transpose.[0].Length do yield row j]

let rec keyXORInner (acc:byte []) (keys:byte []) (bytes:byte []) =
    let transpose = transpose keys.Length bytes
//    Debug.WriteLine (sprintf "%A" transpose)
//    Debug.WriteLine (sprintf "%A" transpose.[0].Length)
//    Debug.WriteLine (sprintf "%A" transpose.[1].Length)
//    Debug.WriteLine (sprintf "%A" transpose.[2].Length)
    let xor = transpose |> Array.mapi (fun i x -> bytesXORSingle x keys.[i])
    let out = revtranspose (keys.Length) xor
    //Debug.WriteLine out.Length
    out
let keyXOR keys bytes = 
    keyXORInner Array.empty keys bytes
//    match bytes with
//    | Nil -> acc
//    | Cons(a,tail) -> keyXORInner (Array.append acc (byteXOR a ([].head infiniteKeys)) ([].skip 1 infiniteKeys) tail

let byteToCount (b:byte) =
    let ba = System.Collections.BitArray([|b|])
    let bools:bool [] = [|false;false;false;false;false;false;false;false|]
    do  ba.CopyTo(bools,0)
    bools |> Array.fold (fun acc x -> if x then acc+1 else acc) 0

let byteEditDistance (b1:byte) (b2:byte) :int=
    let xor = (byteXOR b1 b2).[0]
    byteToCount xor    

let byteArrayEditDistance (ba1:byte []) (ba2:byte []) :int=
    let xors = (ba1,ba2) ||> bytesXOR
    xors |> Array.fold (fun acc x -> acc+(byteToCount x)) 0

let rec bytesEditDistanceInner (acc:int) (bytes1:byte []) (bytes2:byte []) =
    match bytes1, bytes2 with
    | Nil, Nil -> acc
    | Nil, _ | _, Nil -> failwith "Byte arrays should have the same length."
    | Cons(a,taila), Cons(b,tailb) -> bytesEditDistanceInner ((byteEditDistance a b)+acc) taila tailb
let bytesEditDistance bytes1 bytes2 = bytesEditDistanceInner 0 bytes1 bytes2

let rec keyLengthToEditDistanceInner (acc:int) (keyLength:int) (input:byte []) :int=
    if input.Length >= keyLength*2 then
        let one = input.[0..(keyLength-1)]
        let two = input.[keyLength..2*keyLength-1]
        let ed = bytesEditDistance one two
        keyLengthToEditDistanceInner (acc+ed) keyLength (input.[2*keyLength..])
    else acc
let keyLengthToEditDistance keyLength bytes  = ((keyLengthToEditDistanceInner 0 keyLength bytes)|>float)/(1.0|> float)

let rec starHammingInner (keyLength:int) (acc:int) (inner:byte []) (outer:byte []) =
    match inner, outer with
    | Nil, Nil -> acc
    | Nil, Cons(a,tail) -> starHammingInner keyLength acc (outer.[keyLength..]) (outer.[keyLength..])
    | Cons(a,tail), Nil -> failwith "Outer should never be smaller than inner."
    | Cons(a,taila), Cons(b,tailb) -> starHammingInner keyLength (acc+bytesEditDistance inner.[0..keyLength-1] outer.[0..keyLength-1]) inner.[keyLength..] outer
let starHamming keyLength bytes = starHammingInner keyLength 0 bytes bytes

let rec orderKeySizesInner (acc:int []) (currentEDs:float []) (keySizes:int []) (bytes: byte []) =
    //Debug.WriteLine (sprintf "acc = %A" acc)
    //Debug.WriteLine (sprintf "currentEDs = %A" currentEDs)
    match keySizes with
    | Nil -> acc
    | Cons(a,tail) ->
        let newED = keyLengthToEditDistance a bytes
        let index = currentEDs |> Array.tryFindIndex (fun x -> newED < x) 
        match index with
        | Some n -> orderKeySizesInner (Array.concat [acc.[0..n-1];[|a|];acc.[n..acc.Length-2]]) (Array.concat [currentEDs.[0..n-1];[|newED|];currentEDs.[n..currentEDs.Length-2]]) tail bytes
        | None -> orderKeySizesInner acc currentEDs tail bytes
        
let orderKeySizes (margin:int) keySizes bytes = orderKeySizesInner [|for i in 0..margin-1 do yield 0|] [|for i in 0..margin-1 do yield infinity|] keySizes bytes