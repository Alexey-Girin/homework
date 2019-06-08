namespace Homework_8

module Task_2 = 

    open System.Net
    open System.IO
    open System.Text.RegularExpressions

    /// Скачать веб-страницы, на которые указывает начальная веб-страница, напечатать размер каждой
    let explorePages url =

        /// Получить веб-страницу по адресу и напечатать ее размер
        let getPageAsync (url : string) =
            async {
                try 
                    let request = WebRequest.Create(url)
                    use! response = request.AsyncGetResponse()
                    use stream = response.GetResponseStream()
                    use reader = new StreamReader(stream)
                    let page = reader.ReadToEnd()
                    do printfn "%A --- %A" url page.Length
                    return Some page
                with 
                    | _ -> return None
            }
    
        ///  Обзор веб-страниц, на которые указывает начальная
        let getSecondaryPages (initialPage : string) =
            let pattern = "<a\shref=\"?(https?://[a-zA-Z]+[^\"]+)\"?>"
            let regex = Regex(pattern)
            let matches = regex.Matches(initialPage)
            let pages = [for _match in matches -> _match.Groups.[1].ToString() |> getPageAsync]
            Async.Parallel pages |> Async.RunSynchronously |> Array.toList
    
        let initialPage = url |> getPageAsync |> Async.RunSynchronously 
    
        match initialPage with
        | None -> []
        | Some page -> initialPage :: getSecondaryPages page