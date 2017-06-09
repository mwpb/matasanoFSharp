module FileUtils

let fileToStringArray (path:string) = System.IO.File.ReadLines(path) |> Seq.toArray
let fileToStream (path:string) = System.IO.File.OpenRead(path)