module One
//
//open System
//open System.Diagnostics
//open MM
//
////let rec hexCharListToByteArray (hexCharArray:char []) :Error<byte []>=
////    er {
////        do! check (hexCharArray.Length % 2 = 0) (sprintf "Input hex char list has odd length. The char array is: %A" hexCharArray)
////        match hexCharArray with
////        | [||] -> return [||]
////        | [|a|] -> return! Error (sprintf "Input hex char list has odd length. The char list is: %A" hexCharArray)
////        | _ ->
////            let first, second = hexCharArray.[0], hexCharArray.[1]
////            let! (s:byte) = hexPairToByte first second
////            let! (recursion:byte []) = hexCharListToByteArray hexCharArray.[2..]
////            return Array.concat([[|s|];recursion])
////    }
//
////let hexStringToByteArray (hexString:string) :Error<byte []> = hexCharListToByteArray (hexString.ToCharArray())
//
//let byteArrayToHexString (ba:byte []) =
//    let hexString = System.BitConverter.ToString(ba)
//    hexString
//    
//
//let byteToBase64 (b:byte []) = System.Convert.ToBase64String(b)
//let byteToASCII (b:byte) = System.Text.Encoding.ASCII.GetString([|b|])
//
//let hexStringToASCIIString (hexString:string) :Error<string> = 
//    er {
//        let! byteArray = hexStringToByteArray hexString
//        let ASCIIList = byteArray |> Array.map (fun x -> byteToASCII x)
//        return ASCIIList |> String.concat ""
//    }
//
//let hexStringToBase64String (hexString:string) :Error<string> = 
//    er {
//        let! byteList = hexStringToByteArray hexString
//        let base64List = byteList |> byteToBase64
//        return base64List
//    }
//
//let byteXOR (b1:byte) (b2:byte) :Error<byte> = 
//    let ba1 = System.Collections.BitArray([|b1|])
//    let ba2 = System.Collections.BitArray([|b2|])
//    let bits = ba1.Xor(ba2)
//    let (byte:byte []) = [|0b0uy..0b0uy|]
//    do bits.CopyTo(byte,0)
//    OK byte.[0]
//
//let rec arrayOfErrorsToErrorArray (arrayOfErrors:Error<'a> []) :Error<'a []>= 
//    er {
//        match arrayOfErrors with 
//        | [||] -> return [||]
//        | _ -> 
//            let! head = arrayOfErrors.[0]
//            let! recursion = arrayOfErrorsToErrorArray (arrayOfErrors.[1..])
//            return Array.concat [[|head|];recursion]
//    }
//
//let hexStringXOR (s1:string) (s2:string) =
//    er {
//        let! ba1 = hexStringToByteArray s1
//        let! ba2 = hexStringToByteArray s2
//        let newBA = (ba1, ba2) ||> Array.map2 (fun x y -> byteXOR x y)
//        let! ba = newBA |> arrayOfErrorsToErrorArray
//        return (ba |> byteArrayToHexString).Replace("-","")
//    }
//
//let charToByte (c:char) = System.Convert.ToByte(c)
//
//let hexStringXORSingleChar (s1:string) (c:char) =
//    er {
//        let! ba1 = hexStringToByteArray s1
//        let length = ba1.Length
//        let charByte = charToByte c
//        let constantByteArray = [|for i in 0..length/2-1 do yield charByte|]
//        let xorErrors = (constantByteArray,ba1) ||> Array.map2 (fun x y -> byteXOR x y)
//        return! arrayOfErrorsToErrorArray xorErrors
//    }
//    