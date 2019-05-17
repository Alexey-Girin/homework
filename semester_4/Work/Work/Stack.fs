namespace Work

module Stack =

    /// Стек
    type Stack<'a>() = 

        /// Список для хранения элементов стека 
        let mutable list = List<'a>.Empty
    
        /// Добавить элемент в стек 
        member stack.Push(value : 'a) =
            list <- value :: list
        
        /// Проверить стек на пустоту 
        member stack.IsEmpty() =
            list.IsEmpty
    
        /// Изъять элемент стека 
        member stack.Pop() = 
            match list with
            | [] -> failwith "ошибка. стек пуст"
            | head :: tail ->
                list <- tail
                head