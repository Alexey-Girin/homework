namespace Work

module Task2 = 
    
    /// Бинарное дерево 
    type Tree<'a> =
        | Empty
        | Tree of 'a * Tree<'a> * Tree<'a>

    /// Функция, возвращающая все элементы двоичного дерева, удовлетворяющие переданному как параметр условию
    let siftTree tree condition =
        /// Обход дерева и составление списка элементов 
        let rec sift tree =
            match tree with
            | Empty -> List.Empty
            | Tree (node, leftSubtree, rightSubtree) -> 
                if condition node 
                then node :: sift leftSubtree @ sift rightSubtree
                else sift leftSubtree @ sift rightSubtree
        sift tree