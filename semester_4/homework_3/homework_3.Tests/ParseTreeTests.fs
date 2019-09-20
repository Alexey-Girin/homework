namespace homework_3.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_3.ParseTree

[<TestClass>]
type ParseTreeTests () =

    [<TestMethod>]
    member this.``Eval should calculate parse tree`` () =
        eval <| Multiplication (Addition (Subtraction (Value(8.0), Value(1.0)), Division (Value(3.0), Value(2.0))), Value(2.0))
        |> should equal 17.0
    
    [<TestMethod>]
    member this.``Eval should calculate simple parse tree`` () =
        eval <| Value(2.0)
        |> should equal 2.0
    
    [<TestMethod>]
    member this.``Eval should not crash if divide by zero`` () =
        (fun () -> eval <| Division (Value(3.0), Value(0.0)) |> ignore)
        |> should throw typeof<System.DivideByZeroException>

