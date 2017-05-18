module One

open System
open System.Diagnostics
open MainMonad

let hexCharToHalfByte (c:char) :Error<byte> =
    er {
        match c with
        | '0' -> return 0B0uy
        | '1' -> return 0B1uy
        | '2' -> return 0B10uy
        | '3' -> return 0B11uy
        | '4' -> return 0B100uy
        | '5' -> return 0B101uy
        | '6' -> return 0B110uy
        | '7' -> return 0B111uy
        | '8' -> return 0B1000uy
        | '9' -> return 0B1001uy
        | 'a' -> return 0B1010uy
        | 'b' -> return 0B1011uy
        | 'c' -> return 0B1100uy
        | 'd' -> return 0B1101uy
        | 'e' -> return 0B1110uy
        | 'f' -> return 0B1111uy
        | c -> return! Error (sprintf "Input char (%c) is not a hex character." c)
    }

let hexPairToByte (c1:char) (c2:char) :Error<byte>=
    er {
        let! b1 = hexCharToHalfByte c1
        let! b2 = hexCharToHalfByte c2
        return b1*0b10000uy+b2
    }

let rec hexCharListToByteList (hexCharList:char list) :Error<byte list>=
    er {
        do! check (hexCharList.Length % 2 = 0) (sprintf "Input hex char list has odd length. The char list is: %A" hexCharList)
        match hexCharList with
        | [] -> return []
        | a::[] -> return! Error (sprintf "Input hex char list has odd length. The char list is: %A" hexCharList)
        | a::b::tail ->
            let! (s:byte) = hexPairToByte a b
            let! (recursion:byte list) = hexCharListToByteList tail
            return s::recursion
    }

let hexStringToByteList (hexString:string) :Error<byte list> = hexCharListToByteList (hexString.ToCharArray()|>Array.toList)