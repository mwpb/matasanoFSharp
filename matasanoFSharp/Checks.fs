module Checks

let isHexChar (c:char) = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')
let isBase64Char (c:char) = System.Char.IsLetterOrDigit(c) || c='+' || c='/'