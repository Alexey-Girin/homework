namespace homework_8.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_4.Interpreter

[<TestClass>]
type InterpreterTests () =

    [<TestMethod>]
    member this.``Check term wherein several identical variables of different degrees of connectedness`` () = 
        let term = Application (Abstraction ('x',Abstraction ('x',Variable 'x')),Variable 'y')
        let result = Abstraction ('X', Variable('X'))
        term |> reduction |> should equal result
    
    [<TestMethod>]
    member this.``Check term wherein free and bound variables are the same case insensitive, Bound variable is uppercase`` () = 
        let term = Application
                    (Abstraction ('X',Application (Variable 'x',Variable 'X')),Variable 'A')
        let result = Application (Variable('x'), Variable('a'))
        term |> reduction |> should equal result

    [<TestMethod>]
    member this.``Check term wherein free and bound variables are the same case insensitive, Bound variable is  lowercase`` () = 
        let term = Application
                    (Abstraction ('x',Application (Variable 'X',Variable 'x')),Variable 'A')
        let result = Application (Variable('x'), Variable('a'))
        term |> reduction |> should equal result
    
    [<TestMethod>]
    member this.``Check entangled term `` () = 
        let term = Application
                      (Abstraction
                         ('X',
                          Application
                            (Abstraction ('X',Application (Variable 'X',Variable 'X')),Variable 'x')),
                       Variable 'x')
        let result = Application (Variable('x'), Variable('x'))
        term |> reduction |> should equal result
    
    [<TestMethod>]
    member this.``Check term wherein free and bound variables are the same case insensitive`` () = 
        let term = Application
                    (Abstraction ('x',Application (Variable 'X',Variable 'x')),Variable 'X')
        let result = Application (Variable('x'), Variable('x'))
        term |> reduction |> should equal result
    
    [<TestMethod>]
    member this.``Check term wherein name of free and bound variables matches`` () = 
        let term = Application
                      (Abstraction ('x',Abstraction ('y',Application (Variable 'x',Variable 'y'))),
                       Variable 'y')
        let result = Abstraction ('Y',Application (Variable 'y',Variable 'Y'))
        term |> reduction |> should equal result