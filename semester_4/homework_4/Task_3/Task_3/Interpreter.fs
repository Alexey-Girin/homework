namespace Homework_4

module Interpreter =

    /// Терм. Множество переменных - {a..z}
    type Term =
        | Variable of char
        | Application of Term * Term
        | Abstraction of char * Term    
    
    /// Переименование связанных переменных. Приведение свободных к нижнему регистру
    let alphaTransform initialTerm =
        let rec transform bypassTerm boundVariables =
            match bypassTerm with
            | Variable var -> 
                if List.exists (fun elem -> elem = var) boundVariables 
                then System.Char.ToUpper(var) |> Variable
                else System.Char.ToLower(var) |> Variable 
            | Application (leftTerm, rightTerm) -> 
                Application (transform leftTerm boundVariables, transform rightTerm boundVariables)
            | Abstraction (var, term) ->
                Abstraction (System.Char.ToUpper(var), transform term (var :: boundVariables))
        transform initialTerm List.Empty
    
    /// Выполнение подстановки 
    let rec substitution var term substitutedTerm =
        match term with
        | Variable x ->
            match x with
            | x when x = var -> substitutedTerm
            | _ -> Variable x
        | Application (leftTerm, rightTerm) ->
            let newLeftTerm = substitution var leftTerm substitutedTerm
            let newRightTerm = substitution var rightTerm substitutedTerm
            Application (newLeftTerm, newRightTerm)
        | Abstraction (_var, _term) ->
            match _var with
            | _var when _var = var -> Abstraction (_var, _term)
            | _ -> Abstraction (_var, substitution var _term substitutedTerm)

    /// Выполнение бета-редукции
    let reduction initialTerm = 
        let rec performReduction bypassTerm =
            match bypassTerm with 
            | Variable var -> Variable var
            | Abstraction (var, term) -> Abstraction (var, performReduction term)
            | Application (leftTerm, rightTerm) -> 
                match leftTerm with
                | Abstraction (var, term) -> 
                    substitution var term rightTerm |> performReduction
                | _ ->
                    let reducedLeftTerm = performReduction leftTerm
                    match reducedLeftTerm with
                    | Abstraction _ -> Application (reducedLeftTerm, rightTerm) |> performReduction 
                    | _ -> Application (reducedLeftTerm, performReduction rightTerm)   
        alphaTransform initialTerm |> performReduction