open System

let findFirstPosition list value = 
    let rec find list counter = 
        match list with
        | [] -> None
        | head :: tail -> if head = value then Some(counter) else find tail (counter + 1)
    find list 0

let checkPalindrome string = 
    let rec parse string resultList = if string = "" then List.rev resultList else parse string.[1..string.Length - 1] (string.[0] :: resultList)
    List.rev <| parse string [] = parse string []

let rec mergeSort list =

    let rec split list firstList secondList = 
        match list with
        | [] -> firstList, secondList
        | [element] -> element :: firstList, secondList
        | _ -> split list.Tail.Tail (list.Head :: firstList) (list.Tail.Head :: secondList)
        
    let rec merge firstList secondList resultList = 
        match firstList, secondList with
        | [], [] -> resultList
        | [], _ -> List.rev <| List.rev secondList @ resultList
        | _, [] -> List.rev <| List.rev firstList @ resultList
        | firstListHead :: firstListTail, secondListHead :: secondListTail -> 
            if firstListHead < secondListHead
                then merge firstListTail secondList (firstListHead :: resultList) 
                else merge firstList secondListTail (secondListHead :: resultList)  
                
    match list with
    | [] -> []
    | [_] -> list
    | _ ->
        let firstList, secondList = split list [] [] 
        merge (mergeSort firstList) (mergeSort secondList) []