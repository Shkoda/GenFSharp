// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System



module ValueGenerator = 
    let randomGenerator = new System.Random()

    let randomInt = fun () -> randomGenerator.Next(0,1000)
    let randomString = fun () -> "random_text_" + randomInt().ToString()

    let cast<'T> (inputValue : obj) = 
        match inputValue with
        | :? 'T as typedObject -> typedObject 
        | _ ->  Unchecked.defaultof<'T>  
   
    let inline getRandomOfType<'T> () =
      match typeof<'T> with
      | x when x = typeof<float>  -> 42.0 |> cast<'T>
      | x when x = typeof<int>  -> randomInt() |> cast<'T>
      | _ ->  randomString() |> cast<'T>

    let supportedTypes = [|float.GetType(); int.GetType(); string.GetType()|]
    let randomType = fun() -> supportedTypes.[randomGenerator.Next(supportedTypes.Length)]
    
    type Option<'T> (value: 'T) =
        member this.toString = String.Format("Option<{0}>", value)

    type Tuple<'A, 'B> (first: 'A, second: 'B) =
        member this.toString = String.Format("Tuple<{0}, {1}>", first, second)
   
    type wrapperEnum = Option | Tuple


    let generify = fun (times : int) ->
        let wrappers = Enum.GetValues(typeof<wrapperEnum>)
        let length = wrappers.Length
        let randWrapperType = wrappers(randomGenerator.Next(length))
     




[<EntryPoint>]
let main argv = 

    Console.WriteLine("random {0}", ValueGenerator.getRandomOfType<string>())
    Console.ReadLine()

    0 // return an integer exit code
