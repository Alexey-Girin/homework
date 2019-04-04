namespace Homework_3

module ParseTree = 

    type ParseTree =
        | Value of double 
        | Addition of ParseTree * ParseTree
        | Subtraction of ParseTree * ParseTree
        | Multiplication of ParseTree * ParseTree
        | Division of ParseTree * ParseTree

    let rec eval tree = 
        match tree with
        | Value value -> value
        | Addition (leftSubtree, rightSubtree) -> eval leftSubtree + eval rightSubtree
        | Subtraction (leftSubtree, rightSubtree) -> eval leftSubtree - eval rightSubtree
        | Multiplication (leftSubtree, rightSubtree) -> eval leftSubtree * eval rightSubtree
        | Division (leftSubtree, rightSubtree) -> 
            let divisor = eval rightSubtree
            if divisor = 0.0 then raise (System.DivideByZeroException())
            else (eval leftSubtree) / divisor

