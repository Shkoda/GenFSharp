
namespace Shkoda
open System
module ValueGenerator = 
    let randomGenerator = new System.Random()

    let randomString = fun (arg: string[]) -> arg.[randomGenerator.Next(arg.Length)]

    let randomStartArgument = fun()->randomString([|"\"entropy\""; "\"thermodynamic system\""; "\"dimension\""|])
    let randomSomeFunction = fun()->randomString([|"MapToTarget"; "SelectAsSomethingNew"; "CreateFiles"; "OpenHellGate"|])
    let randomNoneFunction = fun()->randomString([|"GenerateProfit"; "KillYourself"; "ImmolateImproved"; "BuildStairwayToHeaved"|])
    let randomSelectFunction = fun()->randomString([|"TransformIT"; "WaterToVine"; "DustToDust"; "PlumbumToAurum"; "GoldenAge"|])

    type Option<'T> (some : 'T) =
        member this.some = some
    type Tuple<'A, 'B>(first: 'A, second: 'B) = 
        member this.First = first  
        member this.Second = second
    type ValueOrError<'A> (value: 'A) = 
        member this.value = value
    type Either<'A, 'B> (left: 'A, right: 'B) = 
        member this.left = left  
        member this.right = right

    let rec ShortTypeName = fun (arg : Object)->     
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

        if safeArg.IsGenericType
            then genericTypeName safeArg
            else safeArg.Name

    let rec newOfTypeText arg =
        match box arg with
        | :? String -> "\"rnd_str\"" :> Object
        | :? ValueOrError<_> as v-> ValueOrError(newOfTypeText (v.value)) :> Object
        | :? Option<_> as o-> Option( newOfTypeText (o.some)) :> Object
        //todo tuple

        | _ -> "AAAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaaAAAAAAAAAAA":>Object
   

    let generifyWithValueOrError arg classText valueText =
        match box arg with
        | :? ValueOrError<_> as v ->  v, classText, valueText+String.Format("\n\t.ContinueWith({0})", randomSelectFunction())
        | _ ->  
            let currentClass = sprintf "ValueOrError<%s>" classText
            ValueOrError(arg), currentClass, String.Format("{0}\n\t.FromValue({1})", currentClass, valueText)  
    
    let generifyWithOption arg classText valueText = 
        match box arg with
        | :? Option<_> as o ->
            match randomGenerator.Next(2) with
            | 0 -> o, classText, valueText + String.Format("\n\t.Cata({0}, {1})", randomSomeFunction(), randomNoneFunction())
            | _ -> o, classText, valueText + String.Format("\n\t.Select({0})", randomSelectFunction())
        | _ -> Option(arg), sprintf "Option<%s>" classText, valueText + "\n\t.ToOption()"   

    let generifyWithTuple arg classText valueText =
        let t = randomStartArgument()
        let (secondArg, secondText) = t,t
        let firstTypeAsString = classText
        let secondTypeAsString = "String"
        let currentClass =  sprintf "Tuple<%s, %s>" firstTypeAsString secondTypeAsString 
        Tuple(arg, secondArg), currentClass, sprintf "%s\n\t.Create(%s, %s)" currentClass valueText secondText 
        

    type WrapperEnum = 
        |Option = 0
        |Tuple = 1
        |ValueOrError = 2
 //       |Either = 3

    
    let randomWrapper = fun () -> enum<WrapperEnum>(randomGenerator.Next(Enum.GetValues(typeof<WrapperEnum>).Length))

    let generify = fun (arg, classText, valueText) ->
        match randomWrapper() with
        | WrapperEnum.Option -> generifyWithOption arg classText valueText  |> fun(opt, classText, valueText) -> opt :> Object, classText, valueText
        | WrapperEnum.ValueOrError -> generifyWithValueOrError arg classText valueText  |> fun(voe, classText, valueText)  -> voe :> Object, classText, valueText
        | WrapperEnum.Tuple -> generifyWithTuple arg classText valueText  |> fun(t, classText, valueText)  -> t :> Object, classText, valueText
     //   | WrapperEnum.Either -> generifyWithEither arg classText valueText  |> fun(e, classText, valueText)  -> e :> Object, classText, valueText
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
