namespace Homework_7

module Calculate = 

    open System

    /// Workflow, выполняющий вычисления с числами, заданными в виде строк
    type CalculateBuilder() =
        let parse string = try string |> int |> Some with | _ -> None    
        member this.Bind(x : string, f) =
            match parse x with
            | None -> None
            | Some number -> f number
        member this.Return(x) = Some(x)