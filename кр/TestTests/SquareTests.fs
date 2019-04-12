namespace Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Test.Square

[<TestClass>]
type SquareTests () =

    [<TestMethod>]
    member this.``Check getSquare with incorrect data`` () =
        getSquare -1 |> should be Empty
    
    [<TestMethod>]
    member this.``Check getSquare with correct data`` () =
        getSquare 4 |> should equal ["****"; "*  *"; "*  *"; "****"]
