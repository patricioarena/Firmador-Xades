namespace PostInstallTask;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        button1 = new Button();
        textBox1 = new TextBox();
        labelCuentaRegresiva = new Label();
        timerCuentaRegresiva = new System.Windows.Forms.Timer(components);
        button2 = new Button();
        SuspendLayout();
        //
        // button1
        //
        button1.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        button1.Location = new Point(146, 162);
        button1.Name = "button1";
        button1.Size = new Size(106, 23);
        button1.TabIndex = 0;
        button1.Text = "Stop timer (dev)";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        //
        // textBox1
        //
        textBox1.BackColor = SystemColors.Control;
        textBox1.BorderStyle = BorderStyle.None;
        textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        textBox1.ForeColor = SystemColors.InfoText;
        textBox1.Location = new Point(6, 12);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(387, 51);
        textBox1.TabIndex = 1;
        textBox1.Text = "Se requiere un reinicio para finalizar la instalación. \r\nEl sistema se reiniciará en 5 minutos.";
        textBox1.TextAlign = HorizontalAlignment.Center;
        //
        // labelCuentaRegresiva
        //
        labelCuentaRegresiva.AutoSize = true;
        labelCuentaRegresiva.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        labelCuentaRegresiva.Location = new Point(171, 75);
        labelCuentaRegresiva.Name = "labelCuentaRegresiva";
        labelCuentaRegresiva.Size = new Size(51, 30);
        labelCuentaRegresiva.TabIndex = 2;
        labelCuentaRegresiva.Text = "5:00";
        //
        // button2
        //
        button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        button2.Location = new Point(108, 120);
        button2.Name = "button2";
        button2.Size = new Size(178, 36);
        button2.TabIndex = 3;
        button2.Text = "Reiniciar ahora";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        //
        // Form1
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoValidate = AutoValidate.EnablePreventFocusChange;
        ClientSize = new Size(412, 203);
        ControlBox = false;
        Controls.Add(button2);
        Controls.Add(labelCuentaRegresiva);
        Controls.Add(textBox1);
        Controls.Add(button1);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        MaximizeBox = false;
        MaximumSize = new Size(414, 205);
        MinimizeBox = false;
        MinimumSize = new Size(414, 205);
        Name = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private TextBox textBox1;
    private Label labelCuentaRegresiva;
    private System.Windows.Forms.Timer timerCuentaRegresiva;
    private Button button2;
}
