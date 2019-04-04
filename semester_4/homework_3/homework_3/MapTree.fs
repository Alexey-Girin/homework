namespace Homework_3

module MapTree =
    
    type Tree<'a> =
    | Empty
    | Tree of 'a * Tree<'a> * Tree<'a>

    let rec map func tree =
        match tree with
        | Empty -> Empty
        | Tree (node, leftSubtree, rightSubtree) -> Tree (func node, map func leftSubtree, map func rightSubtree)