namespace Lib
open FSharpPlus
open FSharpPlus.Operators
//open Ply
open FSharp.Control.Tasks.Builders
open Fleece.Newtonsoft
open Fleece.Newtonsoft.Operators
open FSharp.Data
open FSharp.Data.HttpRequestHeaders

type Person = {
    Name: string
    Age: int
    Children: Person list
}

type Person with
    static member Create name age children = { Person.Name = name; Age = age; Children = children }

    static member OfJson json =
        match json with
        | JObject o -> Person.Create <!> (o .@ "name") <*> (o .@ "age") <*> (o .@ "children")
        | x -> Decode.Fail.objExpected x

module Say =
  let postThing()=
    Http.RequestString
      ( "http://httpbin.org/post", 
        headers = [ ContentType HttpContentTypes.Json ],
        body = TextRequest """ {"test": 42} """)

  let parseJson(str:string) : Person ParseResult=parseJson str
  let hello name =
    printfn "Hello %s" name
