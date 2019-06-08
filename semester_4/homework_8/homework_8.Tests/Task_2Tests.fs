namespace homework_8.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_8.Task_2

[<TestClass>]
type Task_2Tests () =

    [<TestMethod>]
    member this.``Check invalid page`` () = 
        let pages = explorePages "http://meduza.com"
        pages |> should be Empty
    
    [<TestMethod>]
    member this.``Check valid page`` () = 
        let pages = explorePages "http://hwproj.me/courses/31/terms/1"
        pages.Length |> should equal 26

        for page in pages do
            page.IsNone |> should be False