namespace Homework_2

module FindFirstPosition = 

    let findFirstPosition list value = 
        let rec find list counter = 
            match list with
            | [] -> None
            | head :: tail -> if head = value then Some(counter) else find tail (counter + 1)
        find list 0