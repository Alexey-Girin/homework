namespace Homework_2

module MergeSort = 

    let rec mergeSort list =

        let rec split list firstList secondList = 
            match list with
            | [] -> firstList, secondList
            | [item] -> item :: firstList, secondList
            | firstItem :: secondItem :: tail -> split tail (firstItem :: firstList) (secondItem :: secondList)
        
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
