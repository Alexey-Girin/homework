namespace Homework_7

module Calculate = 

    open System

    let convert (x : string) =
        match Int32.TryParse x with
        | true, value -> Some(value)
        | _ -> None

    /// Workflow, выполняющий вычисления с числами, заданными в виде строк
    type CalculateBuilder() =   
        member this.Bind(x : string, f) =
            let convertedX = convert x
            match convertedX with
            | None -> None
            | Some value -> f value
        member this.Return(x) = Some(x)