module S1Q3

open MM
open System
open System.Diagnostics

let hexStringXORWithChar (hexString:string) (c:char) :Error<char []>=
    er {
        let! bytes = INString.hexStringToByteArray hexString
        let constantCharArray = [|for i in 0..(bytes.Length-1) do yield c|]
        let constantByteErrors = constantCharArray |> Array.map IN.charToByte
        let! constantBytes = constantByteErrors |> MM.arrayOfErrorsToErrorArray
        let xorbytes = (bytes,constantBytes) ||> Array.map2 (fun x y -> ByteOps.byteXOR x y)
        let xorCharErrors = xorbytes |> Array.map OUT.byteToChar
        let! xorChars = xorCharErrors |> MM.arrayOfErrorsToErrorArray
        return xorChars
    }

let hexStringToSingleXORHighestScore (hexString:string) :Error<string>=
    er {
        let! bytes = INString.hexStringToByteArray hexString
        let lowerCase = Freqs.expectedChars |> Array.map (fun (x,y) -> x)
        let upperCase = Freqs.expectedChars |> Array.map (fun (x,y) -> Char.ToUpper x)
        let alphabet = Array.concat [lowerCase;upperCase]
        Debug.WriteLine (sprintf "%A" alphabet)
        let xorCharArrayErrors = alphabet |> (Array.map (hexStringXORWithChar hexString))
        let! xorCharArrays = xorCharArrayErrors |> MM.arrayOfErrorsToErrorArray
        Debug.WriteLine (sprintf "%A" (xorCharArrays |> Array.map (fun x -> String.Join("",x))))
        let addScores = xorCharArrays |> Array.map (fun x -> Freqs.stringScore x,x)
        Debug.WriteLine (sprintf "%A" addScores)
        let max = addScores |> Array.maxBy (fun (n,x) -> 1.0-n)
        return String.Join("",max |> snd)
    }