namespace Test

module Square =

    let getSquare n =
        let getFullString n = String.init n (fun index -> "*")
        let getString n = String.init n (fun index -> if index = 0 || index = n - 1 then "*" else " ")
        let createLine n = List.init n (fun index -> if index = 0 || index = n - 1 then getFullString n else getString n)
        match n with
        | n when n <= 0 -> List.Empty
        | _ -> createLine n

    let rec print (list : List<string>) =
        match list with
        | [] -> true
        | head :: tail -> printfn "%A" head; print tail

    getSquare 10 |> print |> ignore