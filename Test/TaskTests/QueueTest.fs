namespace Test.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open TestNamespace.Queue

[<TestClass>]
type QueueTests () =

    [<TestMethod>]
    member this.``Check Queue`` () =
        let queue = PriorityQueue()
        queue.Add(1, 1) |> should be True
        queue.Add(4, 1) |> should be False
        queue.Add(3, 2) |> should be True
        queue.Add(2, 3) |> should be True
        queue.IsEmpty() |> should be False
        (queue.Get(1)).Value |> should equal 1
        (queue.Get(3)).Value |> should equal 2
        (queue.Get(2)).Value |> should equal 3
        queue.IsEmpty() |> should be True


