namespace homework_3.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_3.SequenceOfPrimes

[<TestClass>]
type SequenceOfPrimesTests () =

    [<TestMethod>]
    member this.``Check primes`` () =
        Seq.item 0 generateSequenceOfPrimes |> should equal 2
        Seq.item 4 generateSequenceOfPrimes |> should equal 11
        Seq.item 9 generateSequenceOfPrimes |> should equal 29
        Seq.item 49 generateSequenceOfPrimes |> should equal 229
        Seq.item 99 generateSequenceOfPrimes |> should equal 541
        Seq.item 499 generateSequenceOfPrimes |> should equal 3571
