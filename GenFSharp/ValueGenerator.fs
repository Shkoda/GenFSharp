
namespace Shkoda
open System
module ValueGenerator = 
    let randomGenerator = new System.Random()

    let randomString = fun (arg: string[]) -> arg.[randomGenerator.Next(arg.Length)]

    let randomStartArgument = fun()->randomString([|"\"entropy\""; "\"thermodynamic system\""; "\"dimension\""|])
    let randomSomeFunction = fun()->randomString([|"MapToTarget"; "SelectAsSomethingNew"; "CreateFiles"; "OpenHellGate"|])
    let randomNoneFunction = fun()->randomString([|"GenerateProfit"; "KillYourself"; "ImmolateImproved"; "BuildStairwayToHeaved"|])
    let randomSelectFunction = fun()->randomString([|"GenerateProfit"; "KillYourself"; "ImmolateImproved"; "BuildStairwayToHeaved"|])

    let shortTypeName value = value.GetType().Name
       
    type Option<'T> (some : 'T) =
        member this.some = some
    type Tuple<'A, 'B>(first: 'A, second: 'B) = 
        member this.First = first  
        member this.Second = second
    type ValueOrError<'A> (value: 'A) = 
        member this.value = value

    let typeInfo (t:Type) =
        String.Format("{0} :: IsGenericType={1}, IsGenericTypeDefinition={2}, IsConstructedGenericType={3}, GenericTypeArguments={4}",
                     t.Name, t.IsGenericType, t.IsGenericTypeDefinition, t.IsConstructedGenericType, t.GenericTypeArguments.Length)

    let rec ShortTypeName = fun ( arg : Object)->     
        let argType smth =
            match box smth with
            | :? Type as t -> t 
            | _ -> smth.GetType()     
                  
                  
        let genericTypeName (genType : Type)= 
            let typesAsString types= 
                types 
                |> Array.map(fun t -> ShortTypeName(t))  
                |> String.concat ", "

            let name = genType.Name
            let children = typesAsString (genType.GetGenericArguments())
            sprintf "%s<%s>" name children

        let safeArg = argType arg

        Console.WriteLine("\n"+typeInfo(safeArg)+"\n")

        if safeArg.IsGenericType
            then genericTypeName safeArg
            else safeArg.Name

   

 //   let rec ShortTypeName3 arg =
 //       let t = arg.GetType();
  //      let isTuple =  t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Tuple<_,_>>.GetGenericTypeDefinition()   
          
  //      match box arg with
 //       | :? ValueOrError<_> as v -> "ValueOrError<"+ShortTypeName(v.value)+">"
  //      | :? Option<_> as o -> String.Format("Option<{0}>", ShortTypeName(o.some))
  //      | :? Tuple<_, _> as t -> String.Format("Tuple<{0}, {1}>", ShortTypeName(t.First), ShortTypeName(t.Second))
  //      | _ -> "5555555 "+arg.GetType().Name

    let rec newOfTypeText arg =
        match box arg with
        | :? String -> "\"rnd_str\"" :> Object
        | :? ValueOrError<_> as v-> ValueOrError(newOfTypeText (v.value)) :> Object
        | :? Option<_> as o-> Option( newOfTypeText (o.some)) :> Object
        //todo tuple

        | _ -> "AAAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaaAAAAAAAAAAA":>Object
   

    let generifyWithValueOrError arg classText valueText =
        match box arg with
        | :? ValueOrError<_> as v ->  v, classText, valueText+".ContinueWith(MappingFunction)"
        | _ ->  
            let currentClass = sprintf "ValueOrError<%s>" classText
            ValueOrError(arg), currentClass, String.Format("{0}.FromValue({1})", currentClass, valueText)  
    
    let generifyWithOption arg classText valueText = 
        match box arg with
        | :? Option<_> as o ->
            match randomGenerator.Next(2) with
            | 0 -> o, classText, valueText + String.Format(".Cata({0}, {1})", randomSomeFunction(), randomNoneFunction())
            | _ -> o, classText, valueText + ".Select(SelectionFunc)"
        | _ -> Option(arg), sprintf "Option<%s>" classText, valueText + ".ToOption()"   

    let generifyWithTuple arg  classText valueText =
        let t = randomStartArgument()
        let (secondArg, secondText) = t,t
        let firstTypeAsString = classText
        let secondTypeAsString = "string"
        let currentClass =  sprintf "Tuple<%s, %s>" firstTypeAsString secondTypeAsString 
        Tuple(arg, secondArg), currentClass, sprintf "%s.Create(%s, %s)" currentClass valueText secondText       

    type WrapperEnum = 
        |Option = 0
        |Tuple = 1
        |ValueOrError = 2

    
    let randomWrapper = fun () -> enum<WrapperEnum>(randomGenerator.Next(Enum.GetValues(typeof<WrapperEnum>).Length))
    //let randomWrapper = fun () ->  WrapperEnum.Option

    let generify = fun (arg, classText, valueText) ->
        match randomWrapper() with
        | WrapperEnum.Option -> generifyWithOption arg classText valueText  |> fun(opt, classText, valueText) -> opt :> Object, classText, valueText
        | WrapperEnum.ValueOrError -> generifyWithValueOrError arg classText valueText  |> fun(voe, classText, valueText)  -> voe :> Object, classText, valueText
        | WrapperEnum.Tuple -> generifyWithTuple arg classText valueText  |> fun(t, classText, valueText)  -> t :> Object, classText, valueText
        | _ -> arg, "dsadad", "dsafa"

    let generifyTimes = fun times ->            
        let rec recGenerify = fun (arg, classText, valueText)  counter -> 
            match counter with
            | 0 -> arg, classText, valueText
            | _ -> recGenerify (generify(arg, classText, valueText)) (counter - 1)

        let startArg = randomStartArgument()
        let a = recGenerify(startArg,  startArg.GetType().Name, startArg) (times)
        a

  
(*let form = new Form()
form.Visible <- true
form.Text <- "Generify"

[<STAThread>]
Application.Run(form)*)

//[<EntryPoint>]
//let main argv = 
//    let (arg, classText, valueText) = ValueGenerator.generifyTimes(5)
//    0
  //  Console.WriteLine(` "%s result = %s" classText valueText)


  //  let t = {ValueGenerator.first="a"; ValueGenerator.second="b";}
 //   let tType = t.GetType()
  //  let tupleType = (typedefof<ValueGenerator.Tuple<_,_>>)

  //  let gtType = tType.GetGenericTypeDefinition()
  //  let gtupleType = tupleType.GetGenericTypeDefinition()

 //   Console.WriteLine(String.Format( "tType = {0} \ntupleType = {1}", gtType, gtupleType))


 //       Console.WriteLine()

//        Console.ReadLine()

//        0 // return an integer exit code
