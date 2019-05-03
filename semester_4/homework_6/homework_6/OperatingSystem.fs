namespace Homework_6

module OperatingSystem =

    /// Класс, представляющий операционные системы 
    type OperatingSystem(name : string, probabilityOfVirusInfection : float) =
        
        /// Название ОС
        member val Name = name with get 

        /// Вероятность заражения вирусом (от 0.0 до 1.0)
        member val Risk = probabilityOfVirusInfection with get

