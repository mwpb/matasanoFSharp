﻿module StringAnalysis

open MM
open One

let expectedFrequencies =
    [|
        'a', 8.167
        'b', 1.492
        'c', 2.782
        'd', 4.253
        'e', 12.70
        'f', 2.228    
        'g', 2.015    
        'h', 6.094   
        'i', 6.966    
        'j', 0.153    
        'k', 0.772    
        'l', 4.025    
        'm', 2.406    
        'n', 6.749    
        'o', 7.507    
        'p', 1.929    
        'q', 0.095    
        'r', 5.987    
        's', 6.327    
        't', 9.056    
        'u', 2.758    
        'v', 0.978    
        'w', 2.360    
        'x', 0.150    
        'y', 1.974    
        'z', 0.074
        ' ',19.18182
    |] |> Array.map (fun (x,y) -> (x,y/100.0))

let expectedChars = expectedFrequencies |> Array.map (fun(x,y) -> (x,0))

let sumOfSquaresDifference (freq1:(char*float)[]) (freq2:(char*float)[]) =
    Array.fold2 (fun (acc:float) (a,b) (x,y) -> (float acc) + (b-y)*(b-y)) 0.0 freq1 freq2

let addOccurrence (acc:(char*int) []) (c:char) =
    let index = Array.tryFindIndex (fun (x,y) -> x=c) acc
    acc |> Array.map (fun (ch,m) -> if (c=ch) then (ch,m+1) else (ch,m))

let getCharOccurrencesFromCharArray (input:char []) = 
    Array.fold addOccurrence expectedChars input

let getCharFrequencyFromString (input:string) = 
    let length = input.Length
    let occurrences = getCharOccurrencesFromCharArray (input.ToCharArray())
    occurrences |> Array.map (fun (x,y) -> (x,(float y)/(float length)))

let stringScore (input:string) =
    let inputFrequencies = getCharFrequencyFromString input
    sumOfSquaresDifference inputFrequencies expectedFrequencies

//let tests =
//    [|
//        "zzzzzzzzzzzzzzzzzzzzzzzz"
//        "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"
//        "You can do this by hand. But don't: write code to do it for you. "
//    |] |> Array.iter (fun x -> System.Console.WriteLine (stringScore x))

//let charScoresSingleXOR (input:string) =
//    er {
//        let outErrors = expectedChars |> Array.map (fun (ch,m) -> One.hexStringXORSingleChar input ch)
//        let! errorOuts = One.arrayOfErrorsToErrorArray outErrors 
//        let scores = errorOuts |> Array.map (fun x -> stringScore x)
//        return scores
//    }
//
//let testSingleXOR =
//    [|
//        "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"
//    |] |> Array.iter (fun x -> MM.printError (charScoresSingleXOR x))