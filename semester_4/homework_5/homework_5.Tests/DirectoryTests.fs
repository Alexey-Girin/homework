namespace homework_5.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_5.Directory

[<TestClass>]
type DirectoryTests () =

    [<TestMethod>]
    member this.``Check add to directory`` () = 
        let trueResult = [("name", "phone")]
        [|"name"; "phone"|] |> add List.Empty |> should equal trueResult
    
    [<TestMethod>]
    member this.``Check double add to directory`` () = 
        let trueResult = [("name", "phone"); ("name1", "phone1")]
        let data = [|"name1"; "phone1"|] |> add List.Empty
        [|"name"; "phone"|] |> add data |> should equal trueResult
    
    [<TestMethod>]
    member this.``Check findByName`` () = 
        let trueResult = ["phone"]
        let data = [|"name"; "phone"|] |> add List.Empty
        "name" |> findByName data  |> should equal trueResult
    
    [<TestMethod>]
    member this.``Check findByPhone`` () = 
        let trueResult = ["name"]
        let data = [|"name"; "phone"|] |> add List.Empty
        "phone" |> findByPhone data  |> should equal trueResult