module Constants

let numeralChars = [|'0'..'9'|]
let lowerHexes = [|'a'..'f'|]
let upperHexes = [|'A'..'F'|]
let lowerCaseChars = [|'a'..'z'|]
let upperCaseChars = [|'A'..'Z'|]

let hexChars = Array.concat [numeralChars;lowerHexes;upperHexes]
let base64Chars = Array.concat [lowerCaseChars;upperCaseChars;[|'+'|];[|'/'|]]
let unicodeChars = [|for i in 0..255 do yield System.Convert.ToChar(byte i)|]

let constantBytes (len:int) (b:byte) = [|for i in 1..len do yield b|]

let isHexChar (c:char) = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')
let isBase64Char (c:char) = System.Char.IsLetterOrDigit(c) || c='+' || c='/'