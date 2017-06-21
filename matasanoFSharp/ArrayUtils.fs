module AU

let (|Nil|Cons|) (inArray:'a []) =
    let n = inArray.Length
    if n=0 then Nil
    else Cons(inArray.[0],inArray.[1..])