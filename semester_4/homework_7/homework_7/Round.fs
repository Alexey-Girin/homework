namespace Homework_7

module Round = 

    open System

    /// Workflow, выполняющий математические вычисления с заданной точностью
    type RoundBuilder(digits : int) =
        member this.Bind(x : float, f) =
            Math.Round(x, digits) |> f
        member this.Return(x : float) = 
            Math.Round(x, digits)