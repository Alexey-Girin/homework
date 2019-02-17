let rec factorial x =  
    match x with 
    | 0 -> 1 
    | x when x > 0 -> x * factorial (x - 1) 
    | _ -> failwith "x has incorrect value"

let fibonacci x = 
    let rec fibonacciNumberCalculation firstAuxiliaryValue secondAuxiliaryValue i =
        if i = 1 then secondAuxiliaryValue else
        fibonacciNumberCalculation (firstAuxiliaryValue + secondAuxiliaryValue) (firstAuxiliaryValue) (i - 1)
    match x with 
    | x when x < 1 -> failwith "x has incorrect value" 
    | _ -> fibonacciNumberCalculation 1 1 x

let reverse list = list |> List.fold (fun acc element -> element :: acc) []

let createListOfPowersOfTwo n m =
    let rec getPowerOfTwo degree = 
        match degree with
        | 0 -> 1.0
        | degree when degree > 0 -> 2.0 * getPowerOfTwo (degree - 1)
        | _ -> 1.0 / (getPowerOfTwo -degree)
    let rec createList factor acc m i =
        if i = m then acc else
        createList (factor * 2.0) (factor :: acc) m (i + 1)
    if m < 0 then failwith "m has incorrect value" 
    reverse (createList (getPowerOfTwo n) [] m 0)