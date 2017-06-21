module IN
open System
open System.Diagnostics
open System.Collections
open AU

let charToByte (c:char) = [|System.Convert.ToByte(c)|]

let hexPairToByte (c1:char) (c2:char) =
    [|System.Convert.ToByte(string(c1)+string(c2),16)|]

let base64QuadrupleToBytes (c1:char) (c2:char) (c3:char) (c4:char) =
    let byteArray = System.Convert.FromBase64CharArray([|c1;c2;c3;c4|],0,4)
    [|byteArray.[0];byteArray.[1];byteArray.[2]|]

let rec hexesToBytesInner (acc:byte []) (hexes:char []) =
    match hexes with
    | Nil -> acc
    | Cons(a,Nil) -> failwith "Hex array must be of even length."
    | Cons(a,Cons(b,tail)) -> hexesToBytesInner (Array.append acc (hexPairToByte a b)) tail
let hexesToBytes (hexes:char []) = hexesToBytesInner Array.empty hexes

let rec base64sToBytesInner (acc: byte []) (base64s:char []) =
    match base64s with
    | Nil -> acc
    | Cons(a,Nil) -> failwith "Base64 array must have length that is a multiple of four."
    | Cons(a,Cons(b,Nil)) -> failwith "Base64 array must have length that is a multiple of four."
    | Cons(a,Cons(b,Cons(c,Nil))) -> failwith "Base64 array must have length that is a multiple of four."
    | Cons(a,Cons(b,Cons(c,Cons(d,tail)))) -> base64sToBytesInner (Array.concat [acc;base64QuadrupleToBytes a b c d]) tail
let base64sToBytes (base64s:char []) = hexesToBytesInner Array.empty base64s

let rec charsToBytesInner (acc:byte []) (chars:char []) =
    match chars with
    | Nil -> acc
    | Cons(a,tail) -> charsToBytesInner (Array.append acc (charToByte a)) tail
let charsToBytes (chars:char []) = charsToBytesInner Array.empty chars