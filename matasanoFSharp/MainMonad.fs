module MM

open System
open System.Diagnostics

type Error<'a> =
    | OK of 'a
    | Error of string

type ErrorBuilder() =
    member t.Bind(x:Error<'a>,f:'a -> Error<'b>) =
        match x with
        | OK a -> f(a)
        | Error errorMessage -> Error errorMessage

    member t.Return(x:'a) = OK x
    member t.ReturnFrom(x:Error<'a>) = x

let er = ErrorBuilder()

let check (condition:bool) (message:string) :Error<unit> = 
    match condition with
    | true -> OK ()
    | false -> Error message

let printError (output:Error<'a>) =
    match output with
    | OK a -> Console.WriteLine (sprintf "%A" a)
    | Error s -> Console.WriteLine s

let rec arrayOfErrorsToErrorArray (arrayOfErrors:Error<'a> []) :Error<'a []>= 
    er {
        match arrayOfErrors with 
        | [||] -> return [||]
        | _ -> 
            let! head = arrayOfErrors.[0]
            let! recursion = arrayOfErrorsToErrorArray (arrayOfErrors.[1..])
            return Array.concat [[|head|];recursion]
    }

type Category =
    | OKCaty
    | ErrorCaty

let categorise (error:Error<'a>) =
    match error with
    | OK _ -> OKCaty
    | Error _ -> ErrorCaty