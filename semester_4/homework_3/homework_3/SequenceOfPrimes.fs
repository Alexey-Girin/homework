namespace Homework_3

module SequenceOfPrimes = 

    /// Проверка числа на простоту
    let checkPrime number =
        let limit = number |> double |> sqrt |> int
        number > 1 && seq { 2 .. limit } |> Seq.filter (fun value -> number % value = 0) |> Seq.length = 0
    
    /// Генерация бесконечной последовательности простых чисел
    let generateSequenceOfPrimes () = Seq.filter checkPrime <| Seq.initInfinite int 