namespace homework_3.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_3.NumberOfEven

[<TestClass>]
type NumberOfEvenTests () =

    [<TestMethod>]
    member this.``EvenMap should find number of even numbers in list`` () =
        evenMap [0; 4; 3; -1; -2] |> should equal 3
    
    [<TestMethod>]
    member this.``EvenMap should return 0 with empty list`` () =
        evenMap [] |> should equal 0 

    [<TestMethod>]
    member this.``EvenFilter should find number of even numbers in list`` () =
        evenFilter [0; 4; 3; -1; -2] |> should equal 3
    
    [<TestMethod>]
    member this.``EvenFilter should return 0 with empty list`` () =
        evenFilter [] |> should equal 0 

    [<TestMethod>]
    member this.``EvenFold should find number of even numbers in list`` () =
        evenMap [0; 4; 3; -1; -2] |> should equal 3
    
    [<TestMethod>]
    member this.``EvenFold should return 0 with empty list`` () =
        evenMap [] |> should equal 0 