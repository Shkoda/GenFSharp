// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System



module ValueGenerator = 
    let randomGenerator = new System.Random()
    let strings = [|"aaa"; "bbb"; "ccc"|]
    let randomString = fun () -> strings.[randomGenerator.Next(strings.Length)]

    let shortTypeName value = value.GetType().Name
        
    
    type Option<'T> (value: 'T) =
        member this.Val = value
        member this.wrap = String.Format("Option<{0}>", value)
        member this.create = String.Format("Option<{0}>.Create({1})",shortTypeName(value), value)
    
    type OptionCreateType = New = 0 |Of = 1

          

    type Tuple<'A, 'B> (first: 'A, second: 'B) =
        member this.wrap = String.Format("Tuple<{0}, {1}>", first, second)
        member this.create = String.Format("Tuple<{0}, {1}>.Create({2}, {3})", shortTypeName(first),shortTypeName(second), first, second)
   

    type ValueOrError<'A> (value: 'A) =
        member this.Val = value
        member this.wrap = String.Format("ValueOrError<{0}>", value)
        member this.create = String.Format("ValueOrError<{0}>.FromValue({1})", shortTypeName(value), value)

    let rec newOfTypeText arg =
        match box arg with
        | :? String -> "\"rnd_str\""
        | :? ValueOrError<_> as v-> newOfTypeText (v.Val)
        | :? Option<_> as o-> newOfTypeText (o.Val)
        | _ -> "AAAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaaAAAAAAAAAAA"
   

    let generifyWithValueOrError arg text =
        match box arg with
        | :? ValueOrError<_> as v ->  v, text+".ContinueWith(MappingFunction)"
        | _ ->  ValueOrError(arg), String.Format("ValueOrError<{0}>.FromValue({1})", arg.GetType(), arg)  
    
    let generifyWithOption arg text = 
        match box arg with
        | :? Option<_> as o ->
            match randomGenerator.Next(2) with
            | 0 -> o, String.Format("{0}.Cata(arg -> SomeFunc(arg), {1})", text, newOfTypeText(arg))
            | _ -> o, text + ".Select(SelectionFunc)"
        | _ -> Option(arg), text + ".ToOption()"   


       

    type WrapperEnum = 
        |Option = 0
   //     |Tuple = 1
        |ValueOrError = 1

    
    let randomWrapper = fun () -> enum<WrapperEnum>(randomGenerator.Next(Enum.GetValues(typeof<WrapperEnum>).Length))

    let generifyNew = fun () ->
        let generifyInner = fun(arg:Object, text: string) ->
            match randomWrapper() with
            | WrapperEnum.Option -> generifyWithOption arg text |> fun(opt, str)-> opt:>Object,str
            | WrapperEnum.ValueOrError -> generifyWithValueOrError arg text|> fun(voe, str)-> voe:>Object,str
            | _ -> arg, "err "+text

        generifyInner ("\"init_arg\"", "\"init_arg\"")




    let generify = fun (times : int) ->
        let create = fun() ->
            let rec create = fun accum ->
                match randomWrapper() with
                | WrapperEnum.Option ->(new Option<string>(accum)).create
                | WrapperEnum.ValueOrError ->(new ValueOrError<string>(accum)).create
         //       | WrapperEnum.Tuple ->(new Tuple<string, string>(accum, create(randomString()))).create
                | _ -> accum
            create (randomString())

        let wrap = fun accum ->
            match randomWrapper() with
             | WrapperEnum.Option ->(new Option<string>(accum)).wrap
             | WrapperEnum.ValueOrError ->(new ValueOrError<string>(accum)).wrap
         //    | WrapperEnum.Tuple ->(new Tuple<string, string>(accum, create())).wrap
             | _ -> accum

        let rec recWrap accum count = 
            match count with
            | 0 -> accum
            | _ -> recWrap(wrap(accum))(count-1)

        let someValue = create()
        recWrap someValue times
  



[<EntryPoint>]
let main argv = 
  //  let text = ValueGenerator.generify(10)
    let (arg, text) = ValueGenerator.generifyNew()
    Console.WriteLine(text)
    Console.ReadLine()

    0 // return an integer exit code
