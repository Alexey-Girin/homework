namespace homework_8.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Homework_8.Task_1
open System.ComponentModel
open System.Threading
open System

[<TestClass>]
type Task_1Tests () =

    [<TestMethod>]
    member this.``Check SingleThreadedLazy`` () = 
        let supplier = (fun () -> 1)
        let calculator = LazyFactory.CreateSingleThreadedLazy(supplier)
        calculator.Get() |> should equal 1
    
    [<TestMethod>]
    member this.``SingleThreadedLazy should calсulate only once`` () = 
        let mutable count = 0
        let supplier = (fun () -> 
            count <- count + 1
            count |> should lessThan 2)
        let calculator = LazyFactory.CreateSingleThreadedLazy(supplier)     
        for i in 1 .. 100 do 
            calculator.Get()
    
    [<TestMethod>]
    member this.``Check MultiThreadedLazy`` () = 
        let supplier = (fun () -> 1)
        let worker = new BackgroundWorker()
        worker.DoWork.Add(fun args ->
            let calculator = LazyFactory.CreateMultiThreadedLazy(supplier)
            args.Result <- box <| calculator.Get())
        worker.RunWorkerCompleted.Add(fun args ->
            args.Result |> should equal 1)
        worker.RunWorkerAsync() 
    
    [<TestMethod>]
    member this.``Check LockFreeLaze`` () = 
        let supplier = (fun () -> 1)
        let worker = new BackgroundWorker()
        worker.DoWork.Add(fun args ->
            let calculator = LazyFactory.CreateLockFreeLazy(supplier)
            args.Result <- box <| calculator.Get())
        worker.RunWorkerCompleted.Add(fun args ->
            args.Result |> should equal 1)
        worker.RunWorkerAsync()
    
    [<TestMethod>]
    member this.``LockFreeLaze should calсulate only once`` () =
        let random = new Random()
        let supplier = (fun () -> random.Next()) 
        let calculator = LazyFactory.CreateLockFreeLazy(supplier)
        let mutable results = List.Empty
        let threads = [ for i in 1 .. 100 -> new Thread (fun () -> results <- calculator.Get() :: results) ]
        threads |> List.iter (fun (thread : Thread) -> thread.Start())
        threads |> List.iter (fun (thread : Thread) -> thread.Join())
        let checkResult = calculator.Get()
        for result in results do
            result |> should equal checkResult 