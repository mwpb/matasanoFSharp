module OUTString
open MM
open System

let byteArrayToHexString (byteArray:byte []) :Error<string> =
    er {
        let hexPairErrors = byteArray |> Array.map OUT.byteToHexPair
        let! hexPairs = hexPairErrors |> MM.arrayOfErrorsToErrorArray
        let charArray = hexPairs |> OUT.flatten2
        return String.Join("",charArray)
    }

let byteArrayToBase64String (byteArray:byte []) :Error<string> =
    er {
        let! byteTriples = byteArray |> IN.split3
        let base64QuadrupleErrors = byteTriples |> Array.map (fun (x,y,z) -> OUT.byteTripleToBase64Quadruple x y z)
        let! base64Quadruples = base64QuadrupleErrors |> MM.arrayOfErrorsToErrorArray
        let charArray = base64Quadruples |> OUT.flatten4
        return String.Join("",charArray)
    }

let byteArrayToCharString (byteArray:byte []) :Error<string> =
    er {
        let charErrorArray = byteArray |> Array.map OUT.byteToChar
        let! (charArray:char []) = charErrorArray |> MM.arrayOfErrorsToErrorArray 
        return String.Join("",charArray)
    }