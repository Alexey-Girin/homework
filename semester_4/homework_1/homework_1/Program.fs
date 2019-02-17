let rec factorial x =  
    match x with 
    | 0 -> 1 
    | x when x > 0 -> x * factorial (x - 1) 
    | _ -> -1

let fibonacci x = 
    let rec iter a b i =
        if i = 1 then b else
        iter (a + b) (a) (i - 1)
    match x with 
    | x when x < 1 -> -1 
    | _ -> iter 1 1 x

let rev ls = ls |> List.fold (fun acc elem -> elem :: acc) []

let createList n m =
    let rec pow deg = 
        match deg with
        | 0 -> 1.0
        | deg when deg > 0 -> 2.0 * (pow (deg - 1))
        | _ -> 1.0 / (pow -deg)
    match m with 
    | m when m < 1 -> []
    | _ -> [1 .. m] |> List.map (fun x -> pow (n + x - 1))