module UI

open System
open System.Windows.Forms
open System.Drawing

type MainForm() as form = 
    inherit Form()
    let introduction = new Label()
    let textField = new RichTextBox()
    let generifyButton = new Button()
    let resetButton = new Button()

    do form.InitializeForm

    member this.InitializeForm =
        // Set Form attributes
        this.FormBorderStyle <- FormBorderStyle.Sizable
        this.Text <- "Generify<>"
        this.Width <- 600
        this.Height <- 300

        introduction.Text <- "Помоги Илье создать метод для библиотеки"
        introduction.Location <- new Point(15,10)
        introduction.Dock <- DockStyle.None
        introduction.AutoSize <- true

        textField.Text <- "\"42\".ToOption()"
        textField.Location <- new Point(5,35)
        textField.Width <- 570
        textField.Height <- 165
        textField.Dock <- DockStyle.None
        textField.AutoSize <- true

        generifyButton.Text <- "Generify!"
        generifyButton.Location <- new Point(100,220)
        generifyButton.Width <- 150

        resetButton.Text <- "Reset"
        resetButton.Location <- new Point(370,220)
        resetButton.Width <- 150

        this.Controls.AddRange([| 
                                (introduction:> Control);
                                (textField:> Control);
                                (generifyButton:> Control);
                                (resetButton:> Control);
                               |])

[<STAThread>]
do Application.Run(new MainForm())