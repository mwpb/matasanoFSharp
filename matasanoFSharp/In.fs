module IN
open System
open System.Diagnostics
open System.Collections
open MM

let charToByte (c:char) = System.Convert.ToByte(c)

let hexPairToByte (c1:char) (c2:char) =
    System.Convert.ToByte(string(c1)+string(c2),16)

let base64QuadrupleToBytes (c1:char) (c2:char) (c3:char) (c4:char) =
    let byteArray = System.Convert.FromBase64CharArray([|c1;c2;c3;c4|],0,4)
    seq [byteArray.[0];byteArray.[1];byteArray.[2]]

let rec hexesToBytes (acc:byte seq) (hexes:char seq) =
    match hexes with
    | Nil -> acc
    | Cons(a,Nil) -> failwith "Hex LL must be of even length."
    | Cons(a,Cons(b,tail)) -> hexesToBytes (seq {yield hexPairToByte a b; yield! acc}) tail

let rec base64sToBytes (acc: byte seq) (base64s:char seq) =
    match base64s with
    | Nil -> acc
    | Cons(a,Nil) -> failwith "Base64 LL must have length that is a multiple of four."
    | Cons(a,Cons(b,Nil)) -> failwith "Base64 LL must have length that is a multiple of four."
    | Cons(a,Cons(b,Cons(c,Nil))) -> failwith "Base64 LL must have length that is a multiple of four."
    | Cons(a,Cons(b,Cons(c,Cons(d,tail)))) -> base64sToBytes (seq {yield! base64QuadrupleToBytes a b c d; yield! acc}) tail

let rec charsToBytes (acc:byte seq) (chars:char seq) =
    match chars with
    | Nil -> acc
    | Cons(a,tail) -> charsToBytes (seq {yield charToByte a; yield! acc}) tail