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
        member this.asString = String.Format("Option<{0}>", value)

    type Tuple<'A, 'B> (first: 'A, second: 'B) =
        member this.asString = String.Format("Tuple<{0}, {1}>", first, second)
   

    type ValueOrError<'A> (value: 'A) =
        member this.asString = String.Format("ValueOrError<{0}>", value)
   
    type WrapperEnum = 
        |Option = 0
        |Tuple = 1
        |ValueOrError = 2

    
    let randomWrapper = fun () -> enum<WrapperEnum>(randomGenerator.Next(Enum.GetValues(typeof<WrapperEnum>).Length))

    let generify = fun (times : int) ->
           
        let wrap = fun accum ->
            match randomWrapper() with
             | WrapperEnum.Option ->(new Option<string>(accum)).asString
             | WrapperEnum.Tuple ->(new Tuple<string, int>(accum, getRandomOfType<int>())).asString
             | _ -> accum

        let rec recWrap accum count = 
            match count with
            | 0 -> accum
            | _ -> recWrap(wrap(accum))(count-1)

        let someValue = getRandomOfType<string>()
        recWrap someValue times
  

     


[<EntryPoint>]
let main argv = 
    let text = ValueGenerator.generify(5)
    Console.WriteLine(text)
    Console.ReadLine()

    0 // return an integer exit code
