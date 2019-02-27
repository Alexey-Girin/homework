namespace Homework_2

module CheckPalindrome  =

    let checkPalindrome string = 
        let rec parse string resultList = 
            if string = "" 
                then List.rev resultList 
                else parse string.[1..string.Length - 1] (string.[0] :: resultList)
        List.rev <| parse string [] = parse string []

