module QS

open MM
open System
open System.Diagnostics

(*
Convert hex to base64

The string:

49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d

Should produce:

SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t

So go ahead and make that happen. You'll need to use this code for the rest of the exercises.

Cryptopals Rule:

Always operate on raw bytes, never on encoded strings. Only use hex and base64 for pretty-printing.

*)

let s1q1 (hexString:string) :Error<string>=
    er {
        let! byteArray = INString.hexStringToByteArray hexString
        return! byteArray |> OUTString.byteArrayToBase64String
    }

let s1q1ee (hexString:string) :Error<string>=
    er {
        let! byteArray = INString.hexStringToByteArray hexString
        return! byteArray |> OUTString.byteArrayToCharString
    }

(*

Fixed XOR

Write a function that takes two equal-length buffers and produces their XOR combination.

If your function works properly, then when you feed it the string:

1c0111001f010100061a024b53535009181c

... after hex decoding, and when XOR'd against:

686974207468652062756c6c277320657965

... should produce:

746865206b696420646f6e277420706c6179

*)

let s1q2 (h1:string) (h2:string) :Error<string>=
    er {
        let! bytes1 = INString.hexStringToByteArray h1
        let! bytes2 = INString.hexStringToByteArray h2
        let xorByteErrors = (bytes1,bytes2) ||> Array.map2 ByteOps.byteXOR
        let xorBytes = xorByteErrors
        return! xorBytes |> OUTString.byteArrayToHexString
    }

(*

Single-byte XOR cipher

The hex encoded string:

1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736

... has been XOR'd against a single character. Find the key, decrypt the message.

You can do this by hand. But don't: write code to do it for you.

How? Devise some method for "scoring" a piece of English plaintext. Character frequency is a good metric. Evaluate each output and choose the one with the best score.
Achievement Unlocked

You now have our permission to make "ETAOIN SHRDLU" jokes on Twitter.

*)

let s1q3inner (ba:byte []) (current:Error<byte []>)  (b:byte) :Error<byte []>=
    er {
        let! curr = current
        let xor = ByteOps.byteArraySingleXOR ba b
        return! Freqs.moreLikely xor curr
    }

let s1q3bytes (ba:byte []) =
    er {
        let! alphabetBytes = Constants.base64Chars |> Array.map IN.charToByte |> MM.arrayOfErrorsToErrorArray
        let! mostLikely = alphabetBytes |> Array.fold (s1q3inner ba) (OK [||])
        return mostLikely
    }

let s1q3 (hexString:string) :Error<string> =
    er {
        let! bytes = INString.hexStringToByteArray hexString
        let! mostLikely = s1q3bytes bytes
        let! outChars = mostLikely |> Array.map OUT.byteToChar |> MM.arrayOfErrorsToErrorArray
        return String.Join("", outChars)
    }

(*

Detect single-character XOR

One of the 60-character strings in this file (4.txt) has been encrypted by single-character XOR.

Find it.

(Your code from #3 should help.)

*)

let s1q4inner (current:Error<byte []>) (ba:byte []) :Error<byte []>=
    er {
        let! curr = current
        let! mostLikely = s1q3bytes ba
        return! Freqs.moreLikely mostLikely curr
    }

let s1q4() :Error<string> =
    er {
        let input = FileUtils.fileToStringArray ((__SOURCE_DIRECTORY__)+"/4.txt")
        let hexStrings = input.[0..5]
        let! bytes = hexStrings |> Array.map INString.hexStringToByteArray |> MM.arrayOfErrorsToErrorArray
        let! mostLikely = bytes |> Array.fold (s1q4inner) (OK [||])
        let! outChars = mostLikely |> Array.map OUT.byteToChar |> MM.arrayOfErrorsToErrorArray
        return String.Join("", outChars)
    }