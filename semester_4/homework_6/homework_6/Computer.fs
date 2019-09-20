namespace Homework_6

module Computer =
    
    open OperatingSystem

    /// Класс, представляющий компьютер
    type Computer(id : int, os : OperatingSystem, status : bool) = 
        
        /// Идентификатор компьютера
        member val Id = id with get

        /// Операционная система 
        member val Os = os with get

        /// Статус заражения
        member val Status = status with get, set

