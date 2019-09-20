namespace Homework_6

module Network =
    
    open OperatingSystem
    open Computer
    
    /// Локальная сеть
    type Network(initialConnections : (Computer * Computer list) list) =

        /// Связь компьюетров в сети через список смежности 
        let mutable connections = initialConnections

        /// Список компьютеров, зараженных на данном шаге
        let mutable listOfInfected = List.Empty

        /// Обновление списка смежности после выполнения хода
        let updateConnection () =
            for computer : Computer in listOfInfected do
                computer.Status <- true
            listOfInfected <- List.Empty

        /// Получение состояния сети 
        member network.GetStatus() =
            connections |> List.map (fun (computer, _) -> computer.Id, computer.Status)
        
        /// Выполнение хода 
        member network.MakeMove() =
            let random = new System.Random()
            for computer, connectedComputers in connections do
                let countOfAttempts = 
                    if not computer.Status then
                        connectedComputers 
                        |> List.filter (fun connectedComputer -> connectedComputer.Status) 
                        |> List.length 
                    else 0
                for i = 1 to countOfAttempts do
                    if not computer.Status && random.Next(1, 100) |> float <= computer.Os.Risk * 100.0 then
                        listOfInfected <- computer :: listOfInfected
            updateConnection()