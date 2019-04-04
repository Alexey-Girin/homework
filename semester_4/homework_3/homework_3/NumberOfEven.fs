namespace Homework_3

module NumberOfEven = 

    let evenMap list =
        list |> Seq.map (fun element -> (abs(element) + 1) % 2) |> Seq.sum
    
    let evenFilter list = 
        list |> Seq.filter (fun element -> abs(element) % 2 = 0) |> Seq.length

    let evenFold list =
        list |> Seq.fold (fun acc element -> acc + (abs(element) + 1) % 2) 0