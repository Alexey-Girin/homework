namespace homework_2.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_2.CheckPalindrome

[<TestClass>]
type CheckPalindromeTests () =
    
    [<TestMethod>]
    member this.``CheckPalindrome should return true if string is palindrome`` () =
        checkPalindrome "1234321" |> should be True

    [<TestMethod>]
    member this.``CheckPalindrome should return false if string is not palindrome`` () =
        checkPalindrome "1234" |> should be False

    [<TestMethod>]
    member this.``CheckPalindrome should return true if string is empty`` () =
        checkPalindrome "" |> should be True
        
