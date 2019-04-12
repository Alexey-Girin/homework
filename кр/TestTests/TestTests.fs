namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Test.Test

[<TestClass>]
type SquareTests () =

    [<TestMethod>]
    member this.``Check getSquare with incorrect data`` () =
        getSquare -1 |> should be Empty
    
    [<TestMethod>]
    member this.``Check getSquare with correct data`` () =
        getSquare 4 |> should equal ["****"; "*  *"; "*  *"; "****"]

    [<TestMethod>]
    member this.``Check Fibonacci`` () =
        getSumFibonacci |> should equal 1089154