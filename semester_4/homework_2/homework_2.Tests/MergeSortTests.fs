namespace homework_2.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_2.MergeSort

[<TestClass>]
type MergeSortTests () =

    [<TestMethod>]
    member this.``MergeSort should sort list`` () = 
        mergeSort [3; 5; 1; 4; 2] |> should equal [1; 2; 3; 4; 5]

    [<TestMethod>]
    member this.``MergeSort should sort single item list`` () = 
        mergeSort [1] |> should equal [1]

    [<TestMethod>]
    member this.``MergeSort should not crash if list is empty`` () = 
        mergeSort [] |> should be Empty