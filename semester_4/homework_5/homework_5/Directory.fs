namespace Homework_5

module Directory =

    open System
    open System.IO
    open System.Runtime.Serialization.Formatters.Binary
    
    /// Имя файла
    let fileName = "Data.dat"
    
    /// Добавить запись (имя и телефон)
    let add (data : (string * string) list) (record : string array) =
        if record.Length = 2 
        then 
            let name = record.[0]
            let phone = record.[1]
            (name, phone) :: data
        else 
            data
    
    /// Найти телефон по имени
    let findByName (data : (string * string) list) (name : string) =
        data 
        |> List.fold (fun nameList record -> if fst record = name then snd record :: nameList else nameList) []
    
    /// Найти имя по телефону
    let findByPhone (data : (string * string) list) (phone : string) =
        data 
        |> List.fold (fun phoneList record -> if snd record = phone then fst record :: phoneList else phoneList) []
    
    /// Cохранить текущие данные в файл
    let saveToFile data =
        let fsOut = new FileStream(fileName, FileMode.Create)
        let formatter = new BinaryFormatter()
        formatter.Serialize(fsOut, data)
        fsOut.Close()
    
    /// Считать данные из файла
    let takeFromFile () =
        let fsIn = new FileStream(fileName, FileMode.Open)
        let formatter = new BinaryFormatter()
        let data = unbox<(string * string) list>(formatter.Deserialize(fsIn))
        fsIn.Close()
        data
    
    /// Обработка запросов 
    let rec requestHandler (data : (string * string) list) =
        let request = Console.ReadLine()
        match request with 
        | "1" -> ()
        | "2" -> 
            let record = Console.ReadLine().Split[|' '|]
            requestHandler <| add data record 
        | "3" ->
            let name = Console.ReadLine()
            findByName data name |> printfn "%A"
            requestHandler data
        | "4" -> 
            let phone = Console.ReadLine()
            findByPhone data phone |> printfn "%A"
            requestHandler data
        | "5" ->
            data |> printfn "%A"
            requestHandler data
        | "6" -> 
            saveToFile data
            requestHandler data
        | "7" -> 
            requestHandler <| takeFromFile ()
        | _ -> 
            requestHandler data

    /// Начало работы
    let start () = requestHandler List.Empty