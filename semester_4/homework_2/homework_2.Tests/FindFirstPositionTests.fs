namespace homework_2.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_2.FindFirstPosition

[<TestClass>]
type FindFirstPositionTests () =

    [<TestMethod>]
    member this.``FindFirstPosition should find first position of list item`` () =
        (findFirstPosition [1; 2; 3; 4] 3).Value |> should equal 2 

    [<TestMethod>]
    member this.``FindFirstPosition should return None for value which is not listed`` () =
        findFirstPosition [1; 2; 3; 4] 5 |> should equal None

    [<TestMethod>]
    member this.``FindFirstPosition should return None for empty list`` () =
        findFirstPosition [] 1 |> should equal None