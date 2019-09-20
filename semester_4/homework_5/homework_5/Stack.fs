namespace Homework_5

module Stack =

    /// Стек
    type Stack<'a> =
        | Empty
        | Stack of 'a * Stack<'a>
    
    /// Добавить элемент в стек
    let push value stack =
        Stack (value, stack)
    
    /// Прочитать головной элемент стека
    let top stack =
        match stack with
        | Empty -> None
        | Stack (element, _) -> Some(element)

    /// Удалить головной элемент стека
    let pop stack =
        match stack with
        | Empty -> None
        | Stack (_, residue) -> Some(residue)
    
    /// Проверка стека на пустоту 
    let isEmpty stack =
        (stack = Empty)

