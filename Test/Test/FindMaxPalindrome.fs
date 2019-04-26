namespace TestNamespace

module FindMaxPalindrome =

    ///Поиск максимального палиндрома, полученного произведением двух трёхзначных чисел
    let findMaxPalindrome () =
        let checkPalindrome string = 
            let rec parse string resultList = 
                if string = "" 
                    then List.rev resultList 
                    else parse string.[1..string.Length - 1] (string.[0] :: resultList)
            List.rev <| parse string [] = parse string []
        let rec round ((i : int), (j : int)) = 
            match (i, j) with
            | (100, 100) -> -1 
            | (100, _) -> round (999, j - 1)
            | _ -> if i * j |> string |> checkPalindrome then i * j else round (i - 1, j)
        round (999, 999)
