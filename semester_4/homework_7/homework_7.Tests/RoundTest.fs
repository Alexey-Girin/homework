namespace homework_7.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_7.Round

[<TestClass>]
type RoundTest() =

    [<TestMethod>]
    member this.``Result of calculation should be 0.048``() = 
        let rounding = new RoundBuilder(3)
        rounding {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b
        } |> should equal 0.048

    [<TestMethod>]
    member this.``Result of calculation should be 1.111``() = 
        let rounding = new RoundBuilder(3)
        rounding {
            let! a = 5.0 / 1.5
            let! b = 3.0
            return a / b
        } |> should equal 1.111
    
    [<TestMethod>]
    member this.``Result of calculation should be 3.2``() = 
        let rounding = new RoundBuilder(1)
        rounding {
            let! a = 5.0 + 1.5
            let! b = 2.0
            return a / b
        } |> should equal 3.2
    
    [<TestMethod>]
    member this.``Result of calculation should be 22.5``() = 
        let rounding = new RoundBuilder(1)
        rounding {
            let! a = 5.0 * 1.5
            let! b = 3.0
            return a * b
        } |> should equal 22.5
