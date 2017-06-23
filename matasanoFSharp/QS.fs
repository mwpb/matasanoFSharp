module QS

open AU
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

let s1q1 (hexString:string) = hexString |> Seq.toArray |> IN.hexesToBytes |> OUT.bytesToBase64s |> String.Concat

let s1q1ee (hexString:string) = hexString |> Seq.toArray |> IN.hexesToBytes |> OUT.bytesToChars |> String.Concat

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

let s1q2 (h1:string) (h2:string) = ByteOps.bytesXOR (h1 |> Seq.toArray |> IN.hexesToBytes) (h2 |> Seq.toArray |> IN.hexesToBytes) |> OUT.bytesToHexes

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

let rec s1q3BytesInner (currentScore:float) (currentBytes:byte []) (inBytes:byte []) (alphabetBytes:byte []) =
    match alphabetBytes with
    | Nil -> currentBytes
    | Cons(a,tail) ->
        let xor = ByteOps.bytesXORSingle inBytes a
        let newScore = (Freqs.stringScore xor)
        if newScore < currentScore then s1q3BytesInner (Freqs.stringScore xor) xor inBytes tail
        else s1q3BytesInner currentScore currentBytes inBytes tail
let s1q3Bytes (inBytes:byte []) = s1q3BytesInner 2.0 Array.empty inBytes (Constants.unicodeCharsAsBytes)
let s1q3 (hexString:string) = s1q3Bytes (hexString |> Seq.toArray |> IN.hexesToBytes) |> OUT.bytesToChars |> String.Concat

(*

Detect single-character XOR

One of the 60-character strings in this file (4.txt) has been encrypted by single-character XOR.

Find it.

(Your code from #3 should help.)

*)

//let s1q4inner (current:Error<byte []>) (ba:byte []) :Error<byte []>=
//    er {
//        let! curr = current
//        let! mostLikely = s1q3bytes ba
//        let! out = (Freqs.moreLikely mostLikely curr)
//        return out
//    }

let rec s1q4inner (currentScore:float) (currentBytes:byte []) (lines:byte [] []) =
//    Debug.WriteLine (sprintf "%A" (currentBytes|>OUT.bytesToChars|>String.Concat))
//    Debug.WriteLine currentScore
    match lines with
    | Nil -> currentBytes
    | Cons(a,tail) ->
        if Freqs.stringScore (s1q3Bytes a) < currentScore then s1q4inner (Freqs.stringScore (s1q3Bytes a)) (s1q3Bytes a) tail
        else s1q4inner currentScore currentBytes tail

let s1q4() =
    let hexStrings = System.IO.File.ReadAllLines ((__SOURCE_DIRECTORY__)+"/4.txt")
    let lines = hexStrings |> Array.map (fun x -> x |> Seq.toArray |> IN.hexesToBytes)
    s1q4inner infinity Array.empty lines |> OUT.bytesToChars |> String.Concat

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

let s1q5 (inString:string) =
    ByteOps.keyXOR ("ICE" |> Seq.toArray |>IN.charsToBytes) (inString |> Seq.toArray |>IN.charsToBytes) |> OUT.bytesToHexes |> String.Concat

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

let s1q6part1 (input1:string) (input2:string) = ByteOps.bytesEditDistance (input1|>Seq.toArray|>IN.charsToBytes) (input2|>Seq.toArray|>IN.charsToBytes)
    
let s1q6() = 
    let base64s = System.IO.File.ReadLines ((__SOURCE_DIRECTORY__)+"/6.txt") |> String.Concat
    let bytes = base64s |> Seq.toArray |> IN.base64sToBytes
    let ns = ByteOps.orderKeySizes 3 [|2..40|] bytes
    let ts = ns |> Array.map (fun n -> ByteOps.transpose n bytes)
    let sols = ts |> Array.map (fun t -> t |> Array.map s1q3Bytes)
    let outs = (sols,ns) ||> Array.map2 (fun sol n -> sol |> ByteOps.revtranspose n)
    let out = outs |> Array.map Freqs.stringScore
    let index = out |> Array.mapi (fun i x -> (i,x)) |> Array.minBy(fun (i,x) -> x) |> fst
    outs.[index] |> OUT.bytesToChars |> String.Concat

let s1q7 (key:string) =
    let base64s = System.IO.File.ReadLines ((__SOURCE_DIRECTORY__)+"/7.txt") |> String.Concat
    let keyBytes = key |> Seq.toArray |> IN.charsToBytes
    let bytes = base64s |> Seq.toArray |> IN.base64sToBytes
    AES.aesBytes keyBytes bytes |> OUT.bytesToChars |> String.Concat

let s1q8() =
    let base64s = System.IO.File.ReadLines ((__SOURCE_DIRECTORY__)+"/8.txt") |> Seq.toArray
    let bytes = base64s |> Array.map (Seq.toArray >> IN.base64sToBytes)
    let editDistance = bytes |> Array.map (ByteOps.starHamming 16)
    editDistance |> Array.mapi (fun i x -> (i,x)) |> Array.maxBy snd |> fst