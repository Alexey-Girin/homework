namespace homework_8.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_4.Interpreter

[<TestClass>]
type InterpreterTests () =

    [<TestMethod>]
    member this.``Check simple term`` () = 
        let term = Application (Abstraction ('x',Variable 'x'),Abstraction ('x',Variable 'x'))
        let result = Abstraction ('X',Variable 'X')
        term |> reduction |> should equal result
    
    [<TestMethod>]
    member this.``Check term wherein name of free and bound variables matches`` () = 
        let term = Application
                      (Abstraction ('x',Abstraction ('y',Application (Variable 'x',Variable 'y'))),
                       Variable 'y')
        let result = Abstraction ('Y',Application (Variable 'y',Variable 'Y'))
        term |> reduction |> should equal result
