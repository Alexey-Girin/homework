namespace StackTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Work.Stack

[<TestClass>]
type StackTest() =

    [<TestMethod>]
    member this.``IsEmpty for empty stack should return True``() =
        let stack = new Stack<int>()
        stack.IsEmpty() |> should be True

    [<TestMethod>]
    member this.``IsEmpty for stack with elements should return False``() =
        let stack = new Stack<int>()
        stack.Push 14
        stack.IsEmpty() |> should be False

    [<TestMethod>]
    member this.``IsEmpty should return True``() =
        let stack = new Stack<int>()
        stack.Push 14
        stack.Pop() |> ignore
        stack.IsEmpty() |> should be True
    
    [<TestMethod>]
    member this.``Check Push and Pop``() =
        let stack = new Stack<int>()
        stack.Push 14
        stack.Push 15
        stack.Push 16
        stack.Pop() |> should equal 16
        stack.Pop() |> should equal 15
        stack.Pop() |> should equal 14

    [<TestMethod>]
    member this.``If Pop for empty stack, stack should throw exception``() =
        let stack = new Stack<int>()
        (fun () -> stack.Pop() |> ignore) |> should throw typeof<System.Exception>