namespace Work 

module Task3 =

    /// Хеш-таблица
    type HashTable(hashFunc) =
    
        /// Хеш-функция 
        let hashFunction = hashFunc

        /// Размер таблицы 
        let tableSize = 16

        /// Получить нужный для элемента индекс массива 
        let getIndex value = (hashFunction value) % tableSize

        /// Таблица как массив списков 
        let mutable table = Array.create tableSize List.Empty
    
        /// Добавить элемент в хеш-таблицу
        member hashTable.Add value =
            table.[getIndex value] <- value :: table.[getIndex value]

        /// Проверить наличие элемента в хеш-таблице
        member hashTable.Check value = 
            if table.[getIndex value] = List.Empty
            then false
            else table.[getIndex value] |> List.filter (fun element -> element = value) <> List.Empty
    
        /// Удалить элемент из хеш-таблицы 
        member hashTable.Remove value =
            if hashTable.Check value
            then 
                table.[getIndex value] <- table.[getIndex value] |> List.filter (fun element -> element <> value)
                true 
            else false