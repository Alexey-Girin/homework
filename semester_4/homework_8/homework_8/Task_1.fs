namespace Homework_8

module Task_1 =
    
    /// Интерфейс, представляющий ленивое вычисление 
    type ILazy<'a> =
        abstract member Get: unit -> 'a

    /// Однопоточная версия 
    type SingleThreadedLazy<'a>(supplier : unit -> 'a) =
        let mutable result = None
        interface ILazy<'a> with
            member this.Get() =
                match result with
                | None -> 
                    let currentResult = supplier ()
                    result <- Some currentResult
                    result.Value 
                | Some value -> value
            
    /// Многопоточная версия 
    type MultiThreadedLazy<'a>(supplier : unit -> 'a) =
        let mutable result = None
        let event = new System.Threading.AutoResetEvent(true)
        interface ILazy<'a> with
            member this.Get() =
                event.WaitOne() |> ignore
                try
                    match result with
                    | None -> 
                        let currentResult = supplier ()
                        result <- Some currentResult
                        result.Value
                    | Some value -> value
                finally 
                    event.Set() |> ignore

    /// Lock-free версия 
    type LockFreeLazy<'a>(supplier : unit -> 'a) =
        let mutable result = None
        let mutable isDone = 0
        interface ILazy<'a> with
            member this.Get() =
                while isDone = 0 do
                    let currentResult = supplier ()
                    System.Threading.Interlocked.CompareExchange(&result, Some currentResult, None) |> ignore
                    System.Threading.Interlocked.CompareExchange(&isDone, 1, 0) |> ignore
                result.Value
    
    /// Создает объекты
    type LazyFactory<'a>(supplier : unit -> 'a) =
        static member CreateSingleThreadedLazy supplier = 
            new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>
        static member CreateMultiThreadedLazy supplier = 
            new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>
        static member CreateLockFreeLazy supplier = 
            new SingleThreadedLazy<'a>(supplier) :> ILazy<'a>