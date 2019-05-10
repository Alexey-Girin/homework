namespace Work.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Work.Task2

[<TestClass>]
type Task2Test() =

    [<TestMethod>]
    member this.``Check siftTree with first condition``() = 
        siftTree (Tree (1, Tree (2, Empty, Tree (2, Empty, Empty)), Tree (4, Empty, Empty))) (fun elem -> elem > 1)
        |> should equal [2; 2; 4]
  
    [<TestMethod>]
    member this.``Check siftTree with second condition``() = 
        siftTree (Tree (1, Tree (2, Empty, Tree (2, Empty, Empty)), Tree (4, Empty, Empty))) (fun elem -> elem > 2)
        |> should equal [4]     