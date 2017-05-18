module MainMonad

open System
open System.Diagnostics

type Error<'a> =
    | OK of 'a
    | Error of string

type ErrorBuilder() =
    member t.Bind(x:Error<'a>,f:'a -> Error<'b>) =
        match x with
        | OK a -> f(a)
        | Error errorMessage -> Debug.WriteLine errorMessage; Error errorMessage

    member t.Return(x:'a) = OK x
    member t.ReturnFrom(x:Error<'a>) = x

let er = ErrorBuilder()

let check (condition:bool) (message:string) = 
    match condition with
    | true -> Debug.WriteLine "true"; OK ()
    | false -> Debug.WriteLine "false"; Error message