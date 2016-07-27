// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System



module ValueGenerator = 
    let randomGenerator = new System.Random()
    let randomInt = fun () -> randomGenerator.Next(0,1000)
    let types = dict[int.GetType(), randomInt]






[<EntryPoint>]
let main argv = 
    Console.WriteLine("random {0}", ValueGenerator.randomInt())
    Console.ReadLine()

    0 // return an integer exit code
