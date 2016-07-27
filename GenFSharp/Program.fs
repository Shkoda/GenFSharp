﻿// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System



module ValueGenerator = 
    let randomGenerator = new System.Random()
    let randomInt = fun () -> randomGenerator.Next(0,1000)
    let randomString = fun () -> "tmp"
    let valueGenerators = dict[int.GetType(), randomInt; ]

    let res<'T> = fun (inputValue:obj) -> if (inputValue :? 'T) then inputValue :?> 'T else Unchecked.defaultof<'T>
    let castAs<'T> (o:obj) = 
        match o with
        | :? 'T as res -> res
        | _ -> Unchecked.defaultof<'T>

   // let randomValue = fun () -> 
  //  let inline genFunc<'T when 'T:> Type> ( ) =
    let inline genFunc<'T> ( ) =
      match typeof<'T>  with
        | int -> castAs<'T>(randomInt() )
        | _ ->  castAs<'T>(randomString() )




[<EntryPoint>]
let main argv = 

    Console.WriteLine("random {0}", ValueGenerator.genFunc<string>())
    Console.ReadLine()

    0 // return an integer exit code
