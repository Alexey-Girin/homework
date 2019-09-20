namespace Homework_3

module MapTree =
    
    /// Бинарное дерево
    type Tree<'a> =
    | Empty
    | Tree of 'a * Tree<'a> * Tree<'a>

    /// map для бинарного дерева 
    let rec map func tree =
        match tree with
        | Empty -> Empty
        | Tree (node, leftSubtree, rightSubtree) -> Tree (func node, map func leftSubtree, map func rightSubtree)