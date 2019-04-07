namespace Homework_5

module Directory =

    type Directory() =
        let mutable data : (string * string) list = List.Empty 
        member record.Add (name, phone) = data <- [(name, phone)] @ data
        member record.FindByName name = 
            data |> List.fold (fun nameList record -> if fst record = name then [snd record] @ nameList else nameList) []
        member record.FindByPhone phone = 
            data |> List.fold (fun phoneList record -> if snd record = phone then [fst record] @ phoneList else phoneList) []
        member record.GetAll = data
        member record.Update newData = data <- newData



