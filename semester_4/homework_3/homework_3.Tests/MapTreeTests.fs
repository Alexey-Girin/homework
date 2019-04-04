namespace homework_3.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_3.MapTree

[<TestClass>]
type MapTreeTests () =

    [<TestMethod>]
    member this.``Map should map tree`` () =
        map (fun x -> x + 1) (Tree (1, Tree (1, Empty, Tree (1, Empty, Empty)), Tree (1, Empty, Empty)))
        |> should equal (Tree (2, Tree (2, Empty, Tree (2, Empty, Empty)), Tree (2, Empty, Empty)))
    
    [<TestMethod>]
    member this.``Map should map empty tree`` () =
        map (fun x -> x + 1) (Empty : Tree<int>)
        |> should equal (Empty : Tree<int>)