module InHelpers

open MM

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
        | 'a' | 'A' -> return 0B1010uy
        | 'b' | 'B' -> return 0B1011uy
        | 'c' | 'C' -> return 0B1100uy
        | 'd' | 'D' -> return 0B1101uy
        | 'e' | 'E' -> return 0B1110uy
        | 'f' | 'F' -> return 0B1111uy
        | c -> return! Error (sprintf "Input char (%c) is not a hex character." c)
    }

let isBase64Char (c:char) = System.Char.IsLetterOrDigit(c) || c='+' || c='/'

let base64CharToByte (c:char) =
    er {
        let! index = 
            match c with
            | 'A' -> OK 0
            | 'B' -> OK 1
            | 'C' -> OK 2
            | 'D' -> OK 3
            | 'E' -> OK 4
            | 'F' -> OK 5
            | 'G' -> OK 6
            | 'H' -> OK 7
            | 'I' -> OK 8
            | 'J' -> OK 9
            | 'K' -> OK 10
            | 'L' -> OK 11
            | 'M' -> OK 12
            | 'N' -> OK 13
            | 'O' -> OK 14
            | 'P' -> OK 15
            | 'Q' -> OK 16
            | 'R' -> OK 17
            | 'S' -> OK 18
            | 'T' -> OK 19
            | 'U' -> OK 20
            | 'V' -> OK 21
            | 'W' -> OK 22
            | 'X' -> OK 23
            | 'Y' -> OK 24
            | 'Z' -> OK 25
            | 'a' -> OK 26
            | 'b' -> OK 27
            | 'c' -> OK 28
            | 'd' -> OK 29
            | 'e' -> OK 30
            | 'f' -> OK 31
            | 'g' -> OK 32
            | 'h' -> OK 33
            | 'i' -> OK 34
            | 'j' -> OK 35
            | 'k' -> OK 36
            | 'l' -> OK 37
            | 'm' -> OK 38
            | 'n' -> OK 39
            | 'o' -> OK 40
            | 'p' -> OK 41
            | 'q' -> OK 42
            | 'r' -> OK 43
            | 's' -> OK 44
            | 't' -> OK 45
            | 'u' -> OK 46
            | 'v' -> OK 47
            | 'w' -> OK 48
            | 'x' -> OK 49
            | 'y' -> OK 50
            | 'z' -> OK 51
            | '0' -> OK 52
            | '1' -> OK 53
            | '2' -> OK 54
            | '3' -> OK 55
            | '4' -> OK 56
            | '5' -> OK 57
            | '6' -> OK 58
            | '7' -> OK 59
            | '8' -> OK 60
            | '9' -> OK 61
            | '+' -> OK 62
            | '/' -> OK 63
            | c -> Error (sprintf "Char (%c) is not a base64 character." c)
        System.Diagnostics.Debug.WriteLine index
        return index
    }