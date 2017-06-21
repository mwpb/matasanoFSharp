module Freqs

open AU
open System
open System.Diagnostics

let expectedFrequencies =
    [|'a', 8.167;'b', 1.492;'c', 2.782;'d', 4.253;'e', 12.70;'f', 2.228;'g', 2.015;'h', 6.094;'i', 6.966;'j', 0.153;'k', 0.772;'l', 4.025;'m', 2.406;'n', 6.749;'o', 7.507;'p', 1.929;'q', 0.095;'r', 5.987;'s', 6.327;'t', 9.056;'u', 2.758;'v', 0.978;'w', 2.360;'x', 0.150;'y', 1.974;'z', 0.074;' ',19.18182|]
    |> Array.map (fun (x,y) -> (x,y/100.0))

let zeroFreqs = expectedFrequencies |> Array.map (fun (c,f) -> (c,0.0))

let rec getFreqsInner (freqs:(char*float) []) (input:char []) =
    match input with
    | Nil -> freqs
    | Cons(a,tail) -> 
        let newFreqs = freqs |> Array.map (fun (c,f) -> if c=Char.ToLower(a) then (c,f+1.0) else (c,f))
        getFreqsInner newFreqs tail
let getFreqs (input:byte []) = getFreqsInner zeroFreqs (input|>OUT.bytesToChars) |> Array.map (fun (c,f) -> (c,f/(Seq.length input |>float)))

let rec sumOfSquaresDifference (acc:float) (freq1:(char*float) []) (freq2:(char*float) []) =
    match freq1, freq2 with
    | Nil,Nil -> acc
    | Nil,_ | _,Nil -> failwith "Freqency arrays must be the same size."
    | Cons((c1,f1),taila), Cons((c2,f2),tailb) -> sumOfSquaresDifference (acc+(f1-f2)*(f1-f2)) taila tailb
    
let stringScore (input:byte []) = sumOfSquaresDifference 0.0 expectedFrequencies (getFreqs input)