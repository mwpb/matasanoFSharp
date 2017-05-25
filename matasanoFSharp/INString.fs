module INString
open MM

let hexStringToByteArray (hexString:string) :Error<byte []>=
    er {
        let inChars = hexString.ToCharArray()
        let! inPairs = inChars |> IN.split2
        let byteErrors = inPairs |> Array.map (fun (x,y) -> IN.hexPairToByte x y )
        return! byteErrors |> MM.arrayOfErrorsToErrorArray
    }

let base64StringToByteArray (base64String:string) :Error<byte []>=
    er {
        let inChars = base64String.ToCharArray()
        let! inQuadruples = inChars |> IN.split4
        let byteTripleErrors = inQuadruples |> Array.map (fun (x,y,z,w) -> IN.base64QuadrupleToByteTriple x y z w)
        let! byteTriples = byteTripleErrors |> MM.arrayOfErrorsToErrorArray
        return byteTriples |> OUT.flatten3
    }

let charStringToByteArray (charString:string) :Error<byte []>=
    er {
        let inChars = charString.ToCharArray()
        let byteErrors = inChars |> Array.map IN.charToByte
        return! byteErrors |> MM.arrayOfErrorsToErrorArray
    }