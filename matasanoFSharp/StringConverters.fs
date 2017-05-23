module StringConverters
open MM

let hexStringToBase64String (hexString:string) :Error<string>=
    er {
        let inChars = hexString.ToCharArray()
        let! inPairs = inChars |> Split.split2
        let byteErrors = inPairs |> Array.map (fun (c1,c2) -> IN.hexPairToByte c1 c2)
        let! bytes = byteErrors |> MM.arrayOfErrorsToErrorArray
        let! chars = bytes |> Split.split3
        let base64Errors = chars |> Array.map (fun (b1,b2,b3) -> OUT.byteTripleToBase64Quadruple b1 b2 b3)
        let! base64s = base64Errors |> MM.arrayOfErrorsToErrorArray
        let out = base64s |> Split.flatten4
        return out |> Array.map string |> String.concat ""
    }

let hexStringToCharString (hexString:string) =
    er {
        let inChars = hexString.ToCharArray()
        let! inPairs = inChars |> Split.split2
        let byteErrors = inPairs |> Array.map (fun (c1,c2) -> IN.hexPairToByte c1 c2)
        let! bytes = byteErrors |> MM.arrayOfErrorsToErrorArray
        let charErrors = bytes |>Array.map (fun b -> OUT.byteToChar b)
        let! chars = charErrors |> MM.arrayOfErrorsToErrorArray
        return chars |> Array.map string |> String.concat ""
    }