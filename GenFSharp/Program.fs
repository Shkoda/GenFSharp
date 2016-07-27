// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System



module ValueGenerator = 
    let randomGenerator = new System.Random()
    let randomInt = fun () -> randomGenerator.Next(0,1000)
    let randomString = fun () -> "tmp"
    let valueGenerators = dict[int.GetType(), randomInt; ]
   // let randomValue = fun () -> 
    let inline genFunc<'T when 'T:> Type> (arg : 'T ) =
      match arg with
        | int -> randomInt()
        | _ -> randomString()  




[<EntryPoint>]
let main argv = 

    Console.WriteLine("random {0}", ValueGenerator.genFunc(int.GetType()))
    Console.ReadLine()

    0 // return an integer exit code
