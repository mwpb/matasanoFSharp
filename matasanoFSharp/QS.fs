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
        let xorBytes = (bytes1,bytes2) ||> Array.map2 ByteOps.byteXOR
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
        let alphabetBytes = Constants.unicodeCharsAsBytes
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
        let! out = (Freqs.moreLikely mostLikely curr)
        return out
    }

let s1q4() :Error<string> =
    er {
        let hexStrings = FileUtils.fileToStringArray ((__SOURCE_DIRECTORY__)+"/4.txt")
        let! bytes = hexStrings |> Array.map INString.hexStringToByteArray |> MM.arrayOfErrorsToErrorArray
        let! mostLikely = bytes |> Array.fold (s1q4inner) (OK [||])
        let! out = mostLikely |> OUTString.byteArrayToCharString 
        return out
    }

(*

Implement repeating-key XOR

Here is the opening stanza of an important work of the English language:

Burning 'em, if you ain't quick and nimble
I go crazy when I hear a cymbal

Encrypt it, under the key "ICE", using repeating-key XOR.

In repeating-key XOR, you'll sequentially apply each byte of the key; the first byte of plaintext will be XOR'd against I, the next C, the next E, then I again for the 4th byte, and so on.

It should come out to:

0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272
a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f

Encrypt a bunch of stuff using your repeating-key XOR function. Encrypt your mail. Encrypt your password file. Your .sig file. Get a feel for it. I promise, we aren't wasting your time with this.

*)

let s1q5 (inString:string) :Error<string> =
    er {
        let! bytes = inString |> INString.charStringToByteArray
        let! keyBytes = "ICE" |> INString.charStringToByteArray
        let outBytes = bytes |> ByteOps.keyXOR keyBytes
        let! chars = outBytes |> OUTString.byteArrayToHexString
        return chars
    }

(*

Break repeating-key XOR
It is officially on, now.

This challenge isn't conceptually hard, but it involves actual error-prone coding. The other challenges in this set are there to bring you up to speed. This one is there to qualify you. If you can do this one, you're probably just fine up to Set 6.

There's a file here. It's been base64'd after being encrypted with repeating-key XOR.

Decrypt it.

Here's how:

    Let KEYSIZE be the guessed length of the key; try values from 2 to (say) 40.
    Write a function to compute the edit distance/Hamming distance between two strings. The Hamming distance is just the number of differing bits. The distance between:

    this is a test

    and

    wokka wokka!!!

    is 37. Make sure your code agrees before you proceed.
    For each KEYSIZE, take the first KEYSIZE worth of bytes, and the second KEYSIZE worth of bytes, and find the edit distance between them. Normalize this result by dividing by KEYSIZE.
    The KEYSIZE with the smallest normalized edit distance is probably the key. You could proceed perhaps with the smallest 2-3 KEYSIZE values. Or take 4 KEYSIZE blocks instead of 2 and average the distances.
    Now that you probably know the KEYSIZE: break the ciphertext into blocks of KEYSIZE length.
    Now transpose the blocks: make a block that is the first byte of every block, and a block that is the second byte of every block, and so on.
    Solve each block as if it was single-character XOR. You already have code to do this.
    For each block, the single-byte XOR key that produces the best looking histogram is the repeating-key XOR key byte for that block. Put them together and you have the key.

This code is going to turn out to be surprisingly useful later on. Breaking repeating-key XOR ("Vigenere") statistically is obviously an academic exercise, a "Crypto 101" thing. But more people "know how" to break it than can actually break it, and a similar technique breaks something much more important.
No, that's not a mistake.

We get more tech support questions for this challenge than any of the other ones. We promise, there aren't any blatant errors in this text. In particular: the "wokka wokka!!!" edit distance really is 37.

*)

let s1q6inner() =
    er {
        
        return()
    }

let s1q6() =
    er {
        let first216Chars = FileUtils.firstCharsOfFile 216 (new System.IO.StreamReader((__SOURCE_DIRECTORY__)+"/4.txt"))
        let base64String = first216Chars |> String.Concat
        let! bytes = base64String |> INString.base64StringToByteArray
        let bytes160 = bytes.[0..159]
        let editDistances = [|2..40|] |> Array.map (ByteOps.keyLengthToEditDistance bytes)
        let keySize = editDistances |> Array.mapi (fun i x -> (i+2,x)) |> Array.minBy snd |> fst
        return keySize
    }