namespace TestNamespace

module Queue =

    type PriorityQueue() =
        let mutable queue : (int * int) list = List.Empty 
        member priorityQueue.Add (value, priority) =
            if priorityQueue.FindByPriority (priority) = None
            then queue <- List.sortBy (fun (element, elementPriority) -> elementPriority) ([(value, priority)] @ queue); true
            else false
        member priorityQueue.IsEmpty () = queue  = List.Empty
        member priorityQueue.FindByPriority priority = 
            try Some (List.find (fun (element, elementPriority) -> elementPriority = priority) queue)
            with | :? System.Collections.Generic.KeyNotFoundException -> None
        member priorityQueue.Get priority =
            if priorityQueue.IsEmpty () then failwith "empty queue"
            else if priorityQueue.FindByPriority (priority) <> None
            then queue <- List.filter (fun (element, elementPriority) -> elementPriority <> priority) queue;
                 priorityQueue.FindByPriority (priority)
            else None