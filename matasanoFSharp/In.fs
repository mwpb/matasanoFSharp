module IN

open System
open System.Diagnostics
open System.Collections
open MM
open InHelpers

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

let ofBitArray (bitArray:BitArray) = [|for i=0 to bitArray.Length-1 do yield bitArray.Get(i)|]

let base64QuadrupleToByteTriple (c1:char) (c2:char) (c3:char) (c4:char) =
    er {
        let! ints = [|c1;c2;c3;c4|] |> Array.map (fun c -> (InHelpers.base64CharToByte c)) |> MM.arrayOfErrorsToErrorArray
        Debug.WriteLine (sprintf "%A" ints)
        let byteArray = System.Convert.FromBase64CharArray([|c1;c2;c3;c4|],0,4)
        Debug.WriteLine ("ba length = "+byteArray.Length.ToString())
        return byteArray.[0],byteArray.[1],byteArray.[2]
    }

let tests() = 
    Console.WriteLine "asciiToByte"
    [|'¢';'a';'§';'{'|] |> Array.iter (fun c -> MM.printError (asciiToByte c))
    Console.WriteLine "hexPaiToByte"
    [|('f','2');('g','z');('§','0');('a','a')|] |> Array.iter (fun (c1,c2) -> MM.printError (hexPairToByte c1 c2))
    Console.WriteLine "base64QuadrupleToByteTriple"
    [|
        ('f','2','a','0')
        ('f','§',' ','0')
        ('T','W','F','u')
        ('Y','W','5','5')
        ('I','G','N','h')
        ('c','m','5','h')
        ('b','C','B','w')
        ('b','G','V','h')
        ('c','3','V','y')
        ('f','2',' ','*')
    |] |> Array.iter (fun (c1,c2,c3,c4) -> MM.printError (base64QuadrupleToByteTriple c1 c2 c3 c4)) 