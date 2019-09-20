namespace homework_5.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_5.PointFree

[<TestClass>]
type PointFreeTests () =

    [<TestMethod>]
    member this.``Check func`` () = 
        func 2 [1; 2; 3] |> should equal [2; 4; 6]
  
    [<TestMethod>]
    member this.``Check func'1`` () = 
        func'1 2 [1; 2; 3] |> should equal [2; 4; 6]

    [<TestMethod>]
    member this.``Check func'2`` () = 
        func'2 2 [1; 2; 3] |> should equal [2; 4; 6]

    [<TestMethod>]
    member this.``Check func'3`` () = 
        func'3 2 [1; 2; 3] |> should equal [2; 4; 6]
    
    [<TestMethod>]
    member this.``Check func'4`` () = 
        func'4 2 [1; 2; 3] |> should equal [2; 4; 6]