module FileUtils
open System
open System.IO

let rec firstCharsOfFile (numberRequired:int) (reader:StreamReader) =
    if numberRequired >0 then
        let nextChar = reader.Read() |> char
        if Constants.isBase64Char nextChar then
            Array.concat [[|nextChar|]; firstCharsOfFile (numberRequired-1) reader]
        else firstCharsOfFile (numberRequired) reader
    else [||]

let fileToStringArray (path:string) = System.IO.File.ReadLines(path) |> Seq.toArray
let fileToStream (path:string) = System.IO.File.OpenRead(path)