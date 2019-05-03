namespace Homework_5

open Homework_5.Stack

module BracketSequence =
    
    /// Проверка корректности скобочной последовательности в строке
    let checkBracketSequence string =
        let check bracket stack = 
            if top stack <> Some(bracket) 
            then None 
            else pop stack
        let matching value stack =
            match value with
            | '(' -> Some(push value stack)
            | ')' -> check '(' stack
            | '{' -> Some(push value stack)
            | '}' -> check '{' stack
            | '[' -> Some(push value stack)
            | ']' -> check '[' stack
            | _ -> Some(stack)
        let rec parse list stackOption = 
            if stackOption = None then false 
            else match list with 
                 | [] -> stackOption = Some(Empty)
                 | head :: tail -> parse tail (matching head stackOption.Value)
        parse (string |> Seq.toList) (Some(Empty))