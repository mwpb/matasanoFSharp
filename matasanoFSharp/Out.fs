module OUT
open System
open System.Diagnostics
open MM

let byteToChar (b:byte) :Error<char> =
    er{
        return System.Convert.ToChar(b)
    }

let byteToHexPair (b:byte) :Error<char*char> =
    er {
        let h = System.BitConverter.ToString([|b|]).Replace("-","")
        let a,b = h.[0], h.[1]
        do! check (Checks.isHexChar a) (sprintf "char (%c) is not hex char" a)
        do! check (Checks.isHexChar b) (sprintf "char (%c) is not hex char" b)
        return (a,b)
    }

let byteTripleToBase64Quadruple (b1:byte) (b2:byte) (b3:byte) :Error<char*char*char*char> =
    er {
        let charArray = System.Convert.ToBase64String([|b1;b2;b3|])
        return charArray.[0],charArray.[1],charArray.[2],charArray.[3]
    }