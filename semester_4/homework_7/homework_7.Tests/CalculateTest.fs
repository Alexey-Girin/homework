namespace homework_7.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_7.Calculate

[<TestClass>]
type CalculateTest() =

    [<TestMethod>]
    member this.``Result of calculation should be 3``() = 
        let calculate = new CalculateBuilder()
        calculate {
            let! x = "1"
            let! y = "2"
            let z = x + y
            return z
        } |> should equal (Some 3)

    [<TestMethod>]
    member this.``Result of calculation should be None``() = 
        let calculate = new CalculateBuilder()
        calculate {
            let! x = "1"
            let! y = "b"
            let z = x + y
            return z
        } |> should equal None