namespace homework_3.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_3.SequenceOfPrimes

[<TestClass>]
type SequenceOfPrimesTests () =

    [<TestMethod>]
    member this.``Check primes``() =
        generateSequenceOfPrimes () |> Seq.item 0 |> should equal 2
        generateSequenceOfPrimes () |> Seq.item 4 |> should equal 11
        generateSequenceOfPrimes () |> Seq.item 9 |> should equal 29
        generateSequenceOfPrimes () |> Seq.item 49 |> should equal 229
        generateSequenceOfPrimes () |> Seq.item 99 |> should equal 541
        generateSequenceOfPrimes () |> Seq.item 499 |> should equal 3571
