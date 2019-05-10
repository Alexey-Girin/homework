namespace Work.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit.MsTest
open Work.Task3

[<TestClass>]
type Task3Test() =

    let hashFunction = fun element -> (element |> string |> String.length) * 9675421

    [<TestMethod>]
    member this.``Check with empty hash table``() = 
        let hashTable = new HashTable(hashFunction)

        hashTable.Check "1" |> should be False
        hashTable.Remove "1" |> should be False

    [<TestMethod>]
    member this.``Check hashTable.Add and hashTable.Check``() = 
        let hashTable = new HashTable(hashFunction)
        hashTable.Add "1"
        hashTable.Add "12"

        hashTable.Check "1" |> should be True
        hashTable.Check "12" |> should be True
        hashTable.Check "123" |> should be False
    
    [<TestMethod>]
    member this.``Check hashTable.Remove and hashTable.Check``() = 
        let hashTable = new HashTable(hashFunction)
        hashTable.Add "123"

        hashTable.Check "123" |> should be True

        hashTable.Remove "2" |> should be False
        hashTable.Remove "123" |> should be True

        hashTable.Check "123" |> should be False