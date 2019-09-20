namespace homework_5.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_5.BracketSequence

[<TestClass>]
type BracketSequenceTests () =

    [<TestMethod>]
    member this.``Check empty string`` () = 
        "" |> checkBracketSequence |> should be True
    
    [<TestMethod>]
    member this.``Check simple correct string`` () = 
        "()hb(b((eeg)gre(ewge))tgr((()())(()())_)w(geb)f)g" |> checkBracketSequence |> should be True
    
    [<TestMethod>]
    member this.``Check simple incorrect string`` () = 
        "()())" |> checkBracketSequence |> should not' (be True)
    
    [<TestMethod>]
    member this.``Check correct string with several types of brackets`` () = 
        "(([([])])[()]({({})})){[]()}" |> checkBracketSequence |> should be True
    
    [<TestMethod>]
    member this.``Check incorrect string with several types of brackets`` () = 
        "({)}" |> checkBracketSequence |> should not' (be True)
        