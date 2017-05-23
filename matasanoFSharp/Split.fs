module Split
open MM

let rec split2 (array:'a []) :Error<('a*'a)[]>=
    er {
        if array.Length = 0 then return [||]
        elif (array.Length < 2) then return! Error (sprintf "Length not divisble by 2: %A" array)
        else
            let! recursion = split2 array.[2..] 
            return Array.concat([[|(array.[0],array.[1])|];recursion])
    }

let rec split3 (array:'a []) :Error<('a*'a*'a)[]>=
    er {
        if array.Length = 0 then return [||]
        elif (array.Length < 2) then return! Error (sprintf "Length not divisble by 3: %A" array)
        else
            let! recursion = split3 array.[3..] 
            return Array.concat([[|(array.[0],array.[1],array.[2])|];recursion])
    }

let rec split4 (array:'a []) :Error<('a*'a*'a*'a)[]>=
    er {
        if array.Length = 0 then return [||]
        elif (array.Length < 2) then return! Error (sprintf "Length not divisble by 3: %A" array)
        else
            let! recursion = split4 array.[4..] 
            return Array.concat([[|(array.[0],array.[1],array.[2],array.[3])|];recursion])
    }

let rec flatten2 (array:('a*'a)[]) =
    match array with
    | [||] -> [||]
    | _ -> 
        let a, b = array.[0]
        Array.concat [[|a;b|];flatten2 array.[1..]]

let rec flatten3 (array:('a*'a*'a)[]) =
    match array with
    | [||] -> [||]
    | _ -> 
        let a, b, c = array.[0]
        Array.concat [[|a;b;c|];flatten3 array.[1..]]

let rec flatten4 (array:('a*'a*'a*'a)[]) =
    match array with
    | [||] -> [||]
    | _ -> 
        let a, b, c, d = array.[0]
        Array.concat [[|a;b;c;d|];flatten4 array.[1..]]