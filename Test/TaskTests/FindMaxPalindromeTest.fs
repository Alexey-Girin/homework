namespace Test.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open TestNamespace.FindMaxPalindrome

[<TestClass>]
type FindMaxPalindromeTests () =

    [<TestMethod>]
    member this.``findMaxPalindrome should return 580085`` () =
        findMaxPalindrome () |> should equal 580085
