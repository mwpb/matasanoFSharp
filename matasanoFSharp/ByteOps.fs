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
    Debug.WriteLine (sprintf "%A" key)
    match inBA with
    | [||] -> [||]
    | x -> 
        let head = x.[0]
        let tail = x.[1..]
        let newByte = byteXOR key.[0] head
        Array.concat [[|newByte|]; keyXOR (rotateArray key) tail]