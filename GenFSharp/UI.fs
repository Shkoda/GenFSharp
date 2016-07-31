open System
open System.Windows.Forms
open System.Drawing
open Shkoda.ValueGenerator

module UI =
    let AsMethodText(classText, valueText)  = 
        sprintf "public %s LibraryMethod()\n{\n\t%s\n}" classText valueText

    type MainForm() as form = 
        inherit Form()
        let introduction = new Label()
        let textField = new RichTextBox()
        let generifyButton = new Button()
        let resetButton = new Button()
        let mutable state = Shkoda.ValueGenerator.generifyTimes(1) 

        do form.InitializeForm

      //  member val state = Shkoda.ValueGenerator.generifyTimes(1) with get, set
      
        
        member this.GetCurrentFunctionText = fun () ->
            let (arg, classText, valueText) = state
            AsMethodText(classText, valueText)

        member this.InitializeForm =
            this.FormBorderStyle <- FormBorderStyle.Sizable
            this.Text <- "Generify<>"
            this.Width <- 600
            this.Height <- 300

            introduction.Text <- "Помоги Илье создать метод для библиотеки"
            introduction.Location <- new Point(15,10)
            introduction.Dock <- DockStyle.None
            introduction.AutoSize <- true

            textField.Text <- this.GetCurrentFunctionText()
            textField.Location <- new Point(5,35)
            textField.Width <- 570
            textField.Height <- 165
            textField.Dock <- DockStyle.None
            textField.AutoSize <- true

            generifyButton.Text <- "Generify!"
            generifyButton.Location <- new Point(100,220)
            generifyButton.Width <- 150
            generifyButton.Click.AddHandler(new System.EventHandler 
                (fun s e -> this.generifyClick(s, e)))

            resetButton.Text <- "Reset"
            resetButton.Location <- new Point(370,220)
            resetButton.Width <- 150
            resetButton.Click.AddHandler(new System.EventHandler 
                (fun s e -> this.resetClick(s, e)))


            this.Controls.AddRange([|
                            (introduction:> Control);
                            (textField:> Control);
                            (generifyButton:> Control);
                            (resetButton:> Control);
                               |])
        

        member this.resetClick(sender : System.Object, e : EventArgs) = 
            state <- Shkoda.ValueGenerator.generifyTimes(1)
            textField.Text <- this.GetCurrentFunctionText()

        member this.generifyClick(sender : System.Object, e : EventArgs) = 
            state <- Shkoda.ValueGenerator.generify(state)
            textField.Text <- this.GetCurrentFunctionText()




    [<STAThread>]
    do Application.Run(new MainForm())