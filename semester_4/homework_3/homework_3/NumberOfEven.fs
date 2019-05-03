namespace Homework_3

module NumberOfEven = 

    /// Число четных чисел в списке через map
    let evenMap list =
        list |> Seq.map (fun element -> (abs(element) + 1) % 2) |> Seq.sum
    
    /// Число четных чисел в списке через filter
    let evenFilter list = 
        list |> Seq.filter (fun element -> abs(element) % 2 = 0) |> Seq.length

    /// Число четных чисел в списке через fold
    let evenFold list =
        list |> Seq.fold (fun acc element -> acc + (abs(element) + 1) % 2) 0