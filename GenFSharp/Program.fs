open System


module ValueGenerator = 
    let randomGenerator = new System.Random()
    let strings = [|"\"entropy\""; "\"thermodynamic system\""; "\"dimension\""|]
    let randomString = fun () -> strings.[randomGenerator.Next(strings.Length)]

    let shortTypeName value = value.GetType().Name
       
    type Option<'T> = {some : 'T}
    type Tuple<'A, 'B> = {first: 'A; second: 'B}  
    type ValueOrError<'A> = {value: 'A}

    let rec ShortTypeName arg =
        match box arg with
        | :? ValueOrError<_> as v -> "ValueOrError<"+ShortTypeName(v.value)+">"
        | :? Option<_> as o -> String.Format("Option<{0}>", ShortTypeName(o.some))
        | :? Tuple<_, _> as t -> String.Format("Tuple<{0}, {1}>", ShortTypeName(t.first), ShortTypeName(t.second))
        | _ -> "1111111 "+arg.GetType().Name

    let rec newOfTypeText arg =
        match box arg with
        | :? String -> "\"rnd_str\"" :> Object
        | :? ValueOrError<_> as v-> {value= newOfTypeText (v.value)} :> Object
        | :? Option<_> as o-> {some = newOfTypeText (o.some)} :> Object


        | _ -> "AAAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaaAAAAAAAAAAA":>Object
   

    let generifyWithValueOrError arg text =
        match box arg with
        | :? ValueOrError<_> as v ->  v, text+".ContinueWith(MappingFunction)"
        | _ ->  {value=arg}, String.Format("ValueOrError<{0}>.FromValue({1})", ShortTypeName(arg), text)  
    
    let generifyWithOption arg text = 
        match box arg with
        | :? Option<_> as o ->
            match randomGenerator.Next(2) with
            | 0 -> o, String.Format("{0}.Cata(arg -> SomeFunc(arg), {1})", text, newOfTypeText(arg))
            | _ -> o, text + ".Select(SelectionFunc)"
        | _ -> {some=arg}, text + ".ToOption()"   

    let generifyWithTuple arg text =
        let t = randomString()
        let (secondArg, secondText) = t,t
        let firstTypeAsString = ShortTypeName(arg)
        let secondTypeAsString = ShortTypeName(secondArg)
        {first = arg; second = secondArg}, sprintf "Tuple<%s, %s>.Create(%s, %s)" firstTypeAsString secondTypeAsString text secondText       

    type WrapperEnum = 
        |Option = 0
        |Tuple = 1
        |ValueOrError = 2

    
    let randomWrapper = fun () -> enum<WrapperEnum>(randomGenerator.Next(Enum.GetValues(typeof<WrapperEnum>).Length))

    let generify = fun (arg:Object, text: string) ->
        match randomWrapper() with
        | WrapperEnum.Option -> generifyWithOption arg text |> fun(opt, str)-> opt:>Object,str
        | WrapperEnum.ValueOrError -> generifyWithValueOrError arg text|> fun(voe, str)-> voe:>Object,str
        | WrapperEnum.Tuple -> generifyWithTuple arg text|> fun(t, str)-> t:>Object,str
        | _ -> arg, "errrrrrrrrrrrr "+text

    let generifyTimes = fun times ->            
        let rec recGenerify = fun (arg, text) counter -> 
            match counter with
            | 0 -> arg, text
            | _ -> recGenerify (generify(arg, text)) (counter - 1)

        let startArg = "\"init_arg\""
        recGenerify (startArg, startArg) (times)

  


[<EntryPoint>]
let main argv = 
    let (arg, text) = ValueGenerator.generifyTimes(5)
    let resultType = ValueGenerator.ShortTypeName(arg)
    Console.WriteLine(sprintf "%s result = %s" resultType text)
    Console.ReadLine()

    0 // return an integer exit code
