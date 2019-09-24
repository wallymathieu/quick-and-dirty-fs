namespace Lib
open FSharpPlus
open FSharpPlus.Operators
open FSharp.Control.Tasks.Builders //Ply
open Fleece.Newtonsoft
open Fleece.Newtonsoft.Operators
open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open System.Threading.Tasks
// useful for quick and dirty integration work:
type Somedoc= JsonProvider<"./somedoc.json">

type ISomeTaskBased =
    abstract member Add: int-> Task<int>

type Person = {
    Name: string
    Age: int
    Children: Person list
}
type Country =
| NotSpecified
| England
| Wales
| Scotland
| NorthernIreland
 with static member op_Implicit(c:Country) = 
    match c with | NotSpecified    -> 0
                 | England         -> 1
                 | Wales           -> 2
                 | Scotland        -> 3
                 | NorthernIreland -> 4
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
  let doTaskBasedStuff(taskb:ISomeTaskBased, i:Country)=task{
    let! v= taskb.Add(implicit i)
    return v+1
  }