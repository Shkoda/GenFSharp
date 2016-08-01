open System
open System.Windows.Forms
open System.Drawing
open Shkoda.ValueGenerator

module UI =
    let AsMethodText(classText, valueText)  = 
        sprintf "public %s LibraryMethod()\n{\n    return\n\t%s\n}" classText valueText

    type MainForm() as form = 
        inherit Form()
        let introduction = new Label()
        let textField = new RichTextBox()
        let generifyButton = new Button()
        let resetButton = new Button()
        let mutable state = Shkoda.ValueGenerator.generifyTimes(1) 

        do form.InitializeForm
        
        member this.GetCurrentFunctionText = fun () ->
            let (arg, classText, valueText) = state
            AsMethodText(classText, valueText)

        member this.InitializeForm =
            this.FormBorderStyle <- FormBorderStyle.Sizable
            this.Text <- "Generify<>"
            this.Width <- 800
            this.Height <- 700

            introduction.Text <- "Помоги Илье создать метод для библиотеки =)"
            introduction.Location <- new Point(15,10)
            introduction.Dock <- DockStyle.None
            introduction.AutoSize <- true
            introduction.Font <- new Font("Courier New", 14.0f)

            textField.Text <- this.GetCurrentFunctionText()
            textField.Location <- new Point(5,40)
            textField.Width <- 770
            textField.Height <- 540
            textField.Dock <- DockStyle.None
            textField.AutoSize <- true
            textField.Font <- new Font("Consolas", 10.0f)

            generifyButton.Text <- "Generify!"
            generifyButton.BackColor <- Color.ForestGreen
            generifyButton.Location <- new Point(280,600)
            generifyButton.Width <- 200
            generifyButton.Height <- 50
            generifyButton.Click.AddHandler(new System.EventHandler 
                (fun s e -> this.generifyClick(s, e)))

            resetButton.Text <- "Reset"
            resetButton.Location <- new Point(600,600)
            resetButton.Width <- 150
            resetButton.Height <- 50
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