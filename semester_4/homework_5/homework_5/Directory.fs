namespace Homework_5

module Directory =

    open System
    open System.IO
    open System.Runtime.Serialization.Formatters.Binary

    ///Класс справочника
    type Directory() =
        let mutable data : (string * string) list = List.Empty 
        member record.Add (name, phone) = data <- [(name, phone)] @ data
        member record.FindByName name = 
            data |> List.fold (fun nameList record -> if fst record = name then [snd record] @ nameList else nameList) []
        member record.FindByPhone phone = 
            data |> List.fold (fun phoneList record -> if snd record = phone then [fst record] @ phoneList else phoneList) []
        member record.GetAll = data
        member record.Update newData = data <- newData
    
    ///Имя файла
    let fileName = "Data.dat"
    
    ///Добавить запись (имя и телефон)
    let add (directory : Directory) =
        let record = Console.ReadLine().Split[|' '|]
        if record.Length = 2 then directory.Add((record.[0], record.[1]))
    
    ///Найти телефон по имени
    let findByName (directory : Directory) =
        directory.FindByName(Console.ReadLine()) |> printfn "%A"
    
    ///Найти имя по телефону
    let findByPhone (directory : Directory) =
        directory.FindByPhone(Console.ReadLine()) |> printfn "%A"
    
    ///Вывести всё текущее содержимое базы
    let getAll (directory : Directory) =
        directory.GetAll |> printfn "%A"
    
    ///Cохранить текущие данные в файл
    let saveToFile (directory : Directory) =
        let fsOut = new FileStream(fileName, FileMode.Create)
        let formatter = new BinaryFormatter()
        formatter.Serialize(fsOut, directory.GetAll)
        fsOut.Close()
    
    ///Считать данные из файла
    let takeFromFile (directory : Directory) =
        let fsIn = new FileStream(fileName, FileMode.Open)
        let formatter = new BinaryFormatter()
        let data = unbox<(string * string) list>(formatter.Deserialize(fsIn))
        fsIn.Close()
        directory.Update data
    
    ///Обработка запросов 
    let rec requestHandler (directory : Directory) =
        let request = Console.ReadLine()
        match request with 
        | "1" -> None
        | "2" -> add directory
                 requestHandler directory
        | "3" -> findByName directory
                 requestHandler directory
        | "4" -> findByPhone directory
                 requestHandler directory
        | "5" -> getAll directory
                 requestHandler directory
        | "6" -> saveToFile directory
                 requestHandler directory
        | "7" -> takeFromFile directory 
                 requestHandler directory
        | _ -> requestHandler directory

    let directory = Directory()
    requestHandler directory |> ignore