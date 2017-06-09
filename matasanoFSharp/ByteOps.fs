module ByteOps
open MM

let byteXOR (b1:byte) (b2:byte) = b1^^^b2
let byteArraySingleXOR (ba:byte []) (b:byte) = ba |> Array.map (byteXOR b)
let byteArrayXOR (ba1:byte []) (ba2:byte []) = (ba1,ba2) ||> Array.map2 (fun x y -> byteXOR x y)