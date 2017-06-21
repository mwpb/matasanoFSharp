module OUT
open System
open System.Diagnostics
open AU

let byteToChar (b:byte) = [|System.Convert.ToChar(b)|]

let byteToHexPair (b:byte) =
    let h = System.BitConverter.ToString([|b|]).Replace("-","")
    let a,b = h.[0], h.[1]
    if not (Constants.isHexChar a) then failwith (sprintf "char (%c) is not hex char" a)
    if not (Constants.isHexChar b) then failwith (sprintf "char (%c) is not hex char" b)
    [|a;b|]

let byteTripleToBase64Quadruple (b1:byte) (b2:byte) (b3:byte) =
    let charArray = System.Convert.ToBase64String([|b1;b2;b3|])
    //Debug.WriteLine charArray
    [|charArray.[0];charArray.[1];charArray.[2];charArray.[3]|]

let rec bytesToHexesInner (acc:char []) (bytes:byte []) =
    match bytes with
    | Nil -> acc
    | Cons(a,tail) -> bytesToHexesInner (Array.append acc (byteToHexPair a)) tail
let bytesToHexes (bytes:byte []) = bytesToHexesInner Array.empty bytes

let rec bytesToBase64sInner (acc:char []) (bytes:byte []) =
    match bytes with
    | Nil -> acc
    | Cons(a,Nil) -> failwith "Byte array needs to have length divisible by three."
    | Cons(a,Cons(b,Nil)) -> failwith "Byte array needs to have length divisible by three."
    | Cons(a,Cons(b,Cons(c,tail))) -> bytesToBase64sInner (Array.append acc (byteTripleToBase64Quadruple a b c)) tail
let bytesToBase64s (bytes:byte []) = bytesToBase64sInner Array.empty bytes

let rec bytesToCharsInner (acc:char []) (bytes:byte []) =
    match bytes with
    | Nil -> acc
    | Cons(a,tail) -> bytesToCharsInner (Array.append acc (byteToChar a)) tail
let bytesToChars (bytes:byte []) = bytesToCharsInner Array.empty bytes