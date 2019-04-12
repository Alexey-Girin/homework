namespace Test

module Test =

    ///Поулчить последовательность строк
    let getSquare n =
        let getFullString n = String.init n (fun index -> "*")
        let getString n = String.init n (fun index -> if index = 0 || index = n - 1 then "*" else " ")
        let createLine n = List.init n (fun index -> if index = 0 || index = n - 1 then getFullString n else getString n)
        match n with
        | n when n <= 0 -> List.Empty
        | _ -> createLine n

    ///Распечатать последовательность строк
    let rec print (list : List<string>) =
        match list with
        | [] -> true
        | head :: tail -> printfn "%A" head; print tail

    getSquare 10 |> print |> ignore

    ///Поулчить нужную сумму чисел из последовательности Фибоначчи
    let getSumFibonacci = 
        (0, 1) 
        |> Seq.unfold (fun (firstNumber, secondNumber) -> Some(firstNumber + secondNumber, (secondNumber, firstNumber + secondNumber)))
        |> Seq.takeWhile (fun element -> element < 1000000)
        |> Seq.filter (fun element -> element % 2 = 0)
        |> Seq.sum