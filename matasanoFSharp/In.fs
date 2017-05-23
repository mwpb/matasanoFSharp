module IN

open System
open System.Diagnostics
open System.Collections
open MM
open InHelpers

open NUnit.Framework
open FsUnit

let asciiToByte (c:char) = 
    er{
        do! check (0<=(int c) && (int c) <=127) (sprintf "char %c is not ascii" c)
        return System.Convert.ToByte(c)
    }
    
let hexPairToByte (c1:char) (c2:char) :Error<byte>=
    er {
        let! b1 = hexCharToHalfByte c1
        let! b2 = hexCharToHalfByte c2
        return b1*0b10000uy+b2
    }

let base64QuadrupleToByteTriple (c1:char) (c2:char) (c3:char) (c4:char) =
    er {
        let! ints = [|c1;c2;c3;c4|] |> Array.map (fun c -> (InHelpers.base64CharToByte c)) |> MM.arrayOfErrorsToErrorArray
        Debug.WriteLine (sprintf "%A" ints)
        let byteArray = System.Convert.FromBase64CharArray([|c1;c2;c3;c4|],0,4)
        Debug.WriteLine ("ba length = "+byteArray.Length.ToString())
        return byteArray.[0],byteArray.[1],byteArray.[2]
    }

[<Test>]
let testAsciiToByteSuccess() = 
    [|
        ' ', 0b00100000uy
        '!', 0b00100001uy
        '"', 0b00100010uy
        '#', 0b00100011uy
        '$', 0b00100100uy
        '%', 0b00100101uy
        '&', 0b00100110uy
        ''', 0b00100111uy
        '(', 0b00101000uy
        ')', 0b00101001uy
        '*', 0b00101010uy
        '+', 0b00101011uy
        ',', 0b00101100uy
        '-', 0b00101101uy
        '.', 0b00101110uy
        '/', 0b00101111uy
        '0', 0b00110000uy
        '1', 0b00110001uy
        '2', 0b00110010uy
        '3', 0b00110011uy
        '4', 0b00110100uy
        '5', 0b00110101uy
        '6', 0b00110110uy
        '7', 0b00110111uy
        '8', 0b00111000uy
        '9', 0b00111001uy
        ':', 0b00111010uy
        ';', 0b00111011uy
        '<', 0b00111100uy
        '=', 0b00111101uy
        '>', 0b00111110uy
        '?', 0b00111111uy
        '@', 0b01000000uy
        'A', 0b01000001uy
        'B', 0b01000010uy
        'C', 0b01000011uy
        'D', 0b01000100uy
        'E', 0b01000101uy
        'F', 0b01000110uy
        'G', 0b01000111uy
        'H', 0b01001000uy
        'I', 0b01001001uy
        'J', 0b01001010uy
        'K', 0b01001011uy 
        'L', 0b01001100uy 
        'M', 0b01001101uy 
        'N', 0b01001110uy 
        'O', 0b01001111uy 
        'P', 0b01010000uy 
        'Q', 0b01010001uy 
        'R', 0b01010010uy 
        'S', 0b01010011uy 
        'T', 0b01010100uy 
        'U', 0b01010101uy 
        'V', 0b01010110uy 
        'W', 0b01010111uy 
        'X', 0b01011000uy 
        'Y', 0b01011001uy 
        'Z', 0b01011010uy 
        '[', 0b01011011uy
        '\\', 0b01011100uy
        ']', 0b01011101uy
        '^', 0b01011110uy
        '_', 0b01011111uy
        '`', 0b01100000uy
        'a', 0b01100001uy 
        'b', 0b01100010uy 
        'c', 0b01100011uy 
        'd', 0b01100100uy 
        'e', 0b01100101uy 
        'f', 0b01100110uy 
        'g', 0b01100111uy 
        'h', 0b01101000uy 
        'i', 0b01101001uy 
        'j', 0b01101010uy 
        'k', 0b01101011uy 
        'l', 0b01101100uy 
        'm', 0b01101101uy 
        'n', 0b01101110uy 
        'o', 0b01101111uy 
        'p', 0b01110000uy 
        'q', 0b01110001uy 
        'r', 0b01110010uy 
        's', 0b01110011uy 
        't', 0b01110100uy 
        'u', 0b01110101uy 
        'v', 0b01110110uy 
        'w', 0b01110111uy 
        'x', 0b01111000uy 
        'y', 0b01111001uy 
        'z', 0b01111010uy 
        '{', 0b01111011uy
        '|', 0b01111100uy
        '}', 0b01111101uy
        '~', 0b01111110uy
    |] |> Array.iter (fun (c,b) -> (asciiToByte c) |> should equal (OK b))
[<Test>]
let testAsciiToByteFailure() = 
    [|
        '¢'
        '§'
        '£'
    |] |> Array.iter (fun c -> (asciiToByte c) |> categorise |> (should equal) ErrorCaty)
[<Test>]
let testHexPairToByteSuccess() = 
    [|
        '0','0',0b0000uy
        '0','1',0b0001uy
        '0','2',0b0010uy
        '0','3',0b0011uy
        '0','4',0b0100uy
        '0','5',0b0101uy
        '0','6',0b0110uy
        '0','7',0b0111uy
        '0','8',0b1000uy
        '0','9',0b1001uy
        '0','A',0b1010uy
        '0','B',0b1011uy
        '0','C',0b1100uy
        '0','D',0b1101uy
        '0','E',0b1110uy
        '0','F',0b1111uy
    |] |> Array.iter (fun (x,y,z) -> hexPairToByte x y |> should equal (OK z))
[<Test>]
let testHexPairToByteFailure() = 
    [|
        'z','0',0b0000uy
    |] |> Array.iter (fun (x,y,z) -> hexPairToByte x y |> categorise |> should equal ErrorCaty)
 //    Console.WriteLine "base64QuadrupleToByteTriple"
//    [|
//        ('f','2','a','0')
//        ('f','§',' ','0')
//        ('T','W','F','u')
//        ('Y','W','5','5')
//        ('I','G','N','h')
//        ('c','m','5','h')
//        ('b','C','B','w')
//        ('b','G','V','h')
//        ('c','3','V','y')
//        ('f','2',' ','*')
//    |] |> Array.iter (fun (c1,c2,c3,c4) -> MM.printError (base64QuadrupleToByteTriple c1 c2 c3 c4)) 