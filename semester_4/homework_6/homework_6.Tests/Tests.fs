namespace homework_6.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_6.OperatingSystem
open Homework_6.Computer
open Homework_6.Network

[<TestClass>]
type NetworkTest() =

    let OS = new OperatingSystem("OS", 0.5)
    let badOS = new OperatingSystem("badOS", 1.0)
    let goodOS = new OperatingSystem("goodOS", 0.0)

    [<TestMethod>]
    member this.``Check initial status``() =
        
        let infectedComputer = new Computer(0, OS, true)
        let firstComputer = new Computer(1, badOS, false)
        let secondComputer = new Computer(2, badOS, false)
        let thirdComputer = new Computer(3, badOS, false)

        let initialConnection = 
            [
                infectedComputer, [firstComputer; secondComputer; thirdComputer]
                firstComputer, [infectedComputer]
                secondComputer, [infectedComputer]
                thirdComputer, [infectedComputer]
            ]

        let network = new Network(initialConnection)

        network.GetStatus() |> should equal [(0, true); (1, false); (2, false); (3, false)]

    [<TestMethod>]
    member this.``All network should be infected after first move``() =
        
        let infectedComputer = new Computer(0, OS, true)
        let firstComputer = new Computer(1, badOS, false)
        let secondComputer = new Computer(2, badOS, false)
        let thirdComputer = new Computer(3, badOS, false)

        let initialConnection = 
            [
                infectedComputer, [firstComputer; secondComputer; thirdComputer]
                firstComputer, [infectedComputer]
                secondComputer, [infectedComputer]
                thirdComputer, [infectedComputer]
            ]

        let network = new Network(initialConnection)
        network.MakeMove()

        network.GetStatus() |> should equal [(0, true); (1, true); (2, true); (3, true)]
    
    [<TestMethod>]
    member this.``Just infectedComputer should be infected after first move``() =
        
        let infectedComputer = new Computer(0, OS, true)
        let firstComputer = new Computer(1, goodOS, false)
        let secondComputer = new Computer(2, goodOS, false)
        let thirdComputer = new Computer(3, goodOS, false)

        let initialConnection = 
            [
                infectedComputer, [firstComputer; secondComputer; thirdComputer]
                firstComputer, [infectedComputer]
                secondComputer, [infectedComputer]
                thirdComputer, [infectedComputer]
            ]

        let network = new Network(initialConnection)
        network.MakeMove()

        network.GetStatus() |> should equal [(0, true); (1, false); (2, false); (3, false)]
    
    [<TestMethod>]
    member this.``After second move all network except fourthComputer should be infected``() =
        
        let infectedComputer = new Computer(0, OS, true)
        let firstComputer = new Computer(1, badOS, false)
        let secondComputer = new Computer(2, badOS, false)
        let thirdComputer = new Computer(3, badOS, false)
        let fourthComputer = new Computer(4, goodOS, false)

        let initialConnection = 
            [
                infectedComputer, [firstComputer; secondComputer]
                firstComputer, [infectedComputer; thirdComputer]
                secondComputer, [infectedComputer; fourthComputer]
                thirdComputer, [firstComputer; fourthComputer]
                fourthComputer, [secondComputer; thirdComputer]
            ]

        let network = new Network(initialConnection)
        
        network.GetStatus() |> should equal [(0, true); (1, false); (2, false); (3, false); (4, false)]

        network.MakeMove()
        network.GetStatus() |> should equal [(0, true); (1, true); (2, true); (3, false); (4, false)]

        network.MakeMove()
        network.GetStatus() |> should equal [(0, true); (1, true); (2, true); (3, true); (4, false)]
