module ByteOps
open MM
open System.Diagnostics

let byteXOR (b1:byte) (b2:byte) = b1^^^b2
let byteArraySingleXOR (ba:byte []) (b:byte) = ba |> Array.map (byteXOR b)
let byteArrayXOR (ba1:byte []) (ba2:byte []) = (ba1,ba2) ||> Array.map2 (fun x y -> byteXOR x y)

let rotateArray (inArray:'a []) =
    let head = inArray.[0]
    let tail = inArray.[1..]
    Array.concat [tail;[|head|]]

let rec keyXOR (key:byte []) (inBA:byte []) = 
    Debug.WriteLine key
    match inBA with
    | [||] -> [||]
    | x -> 
        let head = x.[0]
        let tail = x.[1..]
        let newByte = byteXOR key.[0] head
        Array.concat [[|newByte|]; keyXOR (rotateArray key) tail]

let byteToCount (b:byte) =
    let ba = System.Collections.BitArray([|b|])
    let bools:bool [] = [|false;false;false;false;false;false;false;false|]
    do  ba.CopyTo(bools,0)
    bools |> Array.fold (fun acc x -> if x then acc+1 else acc) 0

let byteEditDistance (b1:byte) (b2:byte) :int=
    let xor = byteXOR b1 b2
    byteToCount xor    

let byteArrayEditDistance (ba1:byte []) (ba2:byte []) :int=
    let xors = (ba1,ba2) ||> byteArrayXOR
    xors |> Array.fold (fun acc x -> acc+(byteToCount x)) 0

let keyLengthToEditDistance (input:byte []) (keyLength:int) :float=
    let one = input.[0..(keyLength-1)]
    let two = input.[keyLength..2*keyLength-1]
    let three = input.[2*keyLength..3*keyLength-1]
    let four = input.[3*keyLength..4*keyLength-1]
    let dist1 = byteArrayEditDistance one two |> float
    let dist2 = byteArrayEditDistance three four |> float
    (dist1 + dist2)/(2.0*(keyLength |> float))

let stringEditDistance (s1:string) (s2:string) :Error<int>=
    er {
        let! ba1 = s1 |> INString.charStringToByteArray
        let! ba2 = s2 |> INString.charStringToByteArray
        return byteArrayEditDistance (ba1:byte []) (ba2:byte [])
    }
