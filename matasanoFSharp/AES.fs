module AES
open System
open System.IO
open System.Security.Cryptography

let aesBytes (key:byte []) (bytes:byte []) =
    let aes = new AesManaged()
    aes.Mode <- CipherMode.ECB
    aes.Key <- key
    let decryptor = aes.CreateDecryptor(aes.Key,aes.IV)
    let stream = new MemoryStream()
    let cStream = new CryptoStream(stream,decryptor,CryptoStreamMode.Write)
    cStream.Write(bytes,0,bytes.Length)
    stream.ToArray()