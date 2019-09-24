// Learn more about F# at http://fsharp.org
open FSharpPlus
open System.Linq
open System.IO
open System

[<Diagnostics.CodeAnalysis.SuppressMessage("*", "EnumCasesNames")>]
type Cmd=
  |fetch=0
  |batch=1
type CmdArgs =
  { Command: Cmd option; Dir: string }
let (|Cmd|_|) : _-> Cmd option = tryParse
[<EntryPoint>]
let main argv =
  let defaultArgs = { Command = None; Dir = Directory.GetCurrentDirectory() }
  let usage =
   ["Usage:"
    sprintf "    --dir     DIRECTORY  where to store data (Default: %s)" defaultArgs.Dir
    sprintf "    COMMAND    one of [%s]" (Enum.GetValues( typeof<Cmd> ).Cast<Cmd>() |> Seq.map string |> String.concat ", " )]
    |> String.concat Environment.NewLine
  let rec parseArgs b args =
    match args with
    | [] -> Ok b
    | "--dir" :: dir :: xs -> parseArgs { b with Dir = dir } xs
    | Cmd cmd :: xs-> parseArgs { b with Command = Some cmd } xs
    | invalidArgs ->
      sprintf "error: invalid arguments %A" invalidArgs |> Error

  match argv |> List.ofArray |> parseArgs defaultArgs with
  | Ok args->
    let runSynchronouslyAndPrintResult fn=
      match Async.RunSynchronously fn  with
        | Ok v->
          Console.WriteLine (string v)
          0
        | Error e ->
          Console.Error.WriteLine (string e)
          1

    match args with
    | { Dir=dir; Command=Some command } ->
      match command with
      | Cmd.fetch ->
      
        //Async.RunSynchronously( )
        0
      | Cmd.batch ->

        //Async.RunSynchronously( )
        0
    | _ ->
      printfn "error: Expected command"
      printfn "%s" usage
      1
  | Error err->
      printfn "%s" err
      printfn "%s" usage
      1
