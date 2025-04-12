using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Demo
{
    partial class Signature
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if ((e.CloseReason == CloseReason.WindowsShutDown) || (e.CloseReason == CloseReason.TaskManagerClosing))
                return;

            // Confirm user wants to close
            DialogResult result = MessageBox.Show(this, "        ¿Desea cerrar la aplicación?", "Cerrar", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Hide();
                this.notifyIcon.Visible = true;
                e.Cancel = true;
            }
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Signature));
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            showToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            certPanel = new Panel();
            label3 = new Label();
            label2 = new Label();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            mainPanel = new Panel();
            panel4 = new Panel();
            pictureBox5 = new PictureBox();
            label6 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            settingsPanel = new Panel();
            ontiCheckBox = new CheckBox();
            checkBox1 = new CheckBox();
            panel9 = new Panel();
            serviceStatusPanel = new Panel();
            pictureErrorBox = new PictureBox();
            resetWindowsButton = new Button();
            button1 = new Button();
            settingsButton = new Button();
            rotatedLabel1 = new RotatedLabel();
            pictureBox12 = new PictureBox();
            pictureBox11 = new PictureBox();
            labelVersion = new Label();
            pictureBox7 = new PictureBox();
            certButtonPanel = new Panel();
            contextMenuStrip1.SuspendLayout();
            certPanel.SuspendLayout();
            mainPanel.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            settingsPanel.SuspendLayout();
            serviceStatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureErrorBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip1;
            notifyIcon.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += Form_Show;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { showToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // showToolStripMenuItem
            // 
            showToolStripMenuItem.Name = "showToolStripMenuItem";
            showToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            showToolStripMenuItem.Text = "Abrir";
            showToolStripMenuItem.Click += showToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "pdf";
            openFileDialog1.Filter = "Archivos PDF|*.pdf";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.FileName = "43068e33-ec15-4a7d-aa9c-aa3977e293f7";
            saveFileDialog1.Filter = "Archivos PDF|*.pdf";
            saveFileDialog1.Title = "Guardar archivo PDF";
            // 
            // certPanel
            // 
            certPanel.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            certPanel.Controls.Add(label3);
            certPanel.Controls.Add(label2);
            certPanel.Controls.Add(listView1);
            certPanel.Location = new System.Drawing.Point(0, 195);
            certPanel.Name = "certPanel";
            certPanel.Size = new System.Drawing.Size(413, 224);
            certPanel.TabIndex = 6;
            certPanel.Paint += panel2_Paint;
            // 
            // label3
            // 
            label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(297, 11);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(105, 24);
            label3.TabIndex = 8;
            label3.Text = "Verificación";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(12, 11);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(285, 24);
            label2.TabIndex = 7;
            label2.Text = "Certificados";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listView1
            // 
            listView1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.Location = new System.Drawing.Point(12, 11);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(390, 199);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.KeyDown += ListView_KeyDown;
            listView1.MouseDoubleClick += ListView1MouseDoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Certificados";
            columnHeader1.Width = 273;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Verificación";
            columnHeader2.TextAlign = HorizontalAlignment.Center;
            columnHeader2.Width = 90;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            mainPanel.Controls.Add(panel4);
            mainPanel.Controls.Add(pictureBox1);
            mainPanel.Controls.Add(pictureBox2);
            mainPanel.Controls.Add(settingsPanel);
            mainPanel.Controls.Add(panel9);
            mainPanel.Location = new System.Drawing.Point(1, 194);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new System.Drawing.Size(411, 224);
            mainPanel.TabIndex = 21;
            mainPanel.Paint += panel7_Paint_2;
            // 
            // panel4
            // 
            panel4.BackColor = System.Drawing.Color.Transparent;
            panel4.Controls.Add(pictureBox5);
            panel4.Controls.Add(label6);
            panel4.Location = new System.Drawing.Point(10, 117);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(385, 61);
            panel4.TabIndex = 12;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = System.Drawing.Color.Transparent;
            pictureBox5.Image = Properties.Resources.globe_18dp;
            pictureBox5.Location = new System.Drawing.Point(30, 20);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new System.Drawing.Size(18, 18);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 9;
            pictureBox5.TabStop = false;
            pictureBox5.Click += pictureBox5_Click;
            // 
            // label6
            // 
            label6.Cursor = Cursors.Hand;
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
            label6.ForeColor = System.Drawing.Color.FromArgb(50, 53, 51);
            label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label6.Location = new System.Drawing.Point(54, 21);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(213, 18);
            label6.TabIndex = 6;
            label6.Text = "https://authentica.fepba.gov.ar/";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label6.Click += label6_Click;
            label6.MouseLeave += label6_MouseLeave;
            label6.MouseMove += label6_MouseMove;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.Image = Properties.Resources.logo_fiscalia;
            pictureBox1.Location = new System.Drawing.Point(30, 18);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(192, 41);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.logo_cife_color_v2;
            pictureBox2.Location = new System.Drawing.Point(251, 18);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(105, 41);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // settingsPanel
            // 
            settingsPanel.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            settingsPanel.Controls.Add(ontiCheckBox);
            settingsPanel.Controls.Add(checkBox1);
            settingsPanel.Location = new System.Drawing.Point(0, 1);
            settingsPanel.Name = "settingsPanel";
            settingsPanel.Size = new System.Drawing.Size(413, 208);
            settingsPanel.TabIndex = 22;
            // 
            // ontiCheckBox
            // 
            ontiCheckBox.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            ontiCheckBox.Checked = true;
            ontiCheckBox.CheckState = CheckState.Checked;
            ontiCheckBox.Cursor = Cursors.Hand;
            ontiCheckBox.Enabled = false;
            ontiCheckBox.FlatStyle = FlatStyle.System;
            ontiCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            ontiCheckBox.ForeColor = System.Drawing.Color.FromArgb(50, 53, 51);
            ontiCheckBox.Location = new System.Drawing.Point(169, 33);
            ontiCheckBox.Name = "ontiCheckBox";
            ontiCheckBox.Size = new System.Drawing.Size(80, 20);
            ontiCheckBox.TabIndex = 24;
            ontiCheckBox.Text = "Onti";
            ontiCheckBox.UseVisualStyleBackColor = false;
            ontiCheckBox.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBox1
            // 
            checkBox1.BackColor = System.Drawing.Color.Silver;
            checkBox1.FlatStyle = FlatStyle.Flat;
            checkBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            checkBox1.ForeColor = System.Drawing.Color.FromArgb(50, 53, 51);
            checkBox1.Location = new System.Drawing.Point(247, 168);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(163, 37);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Iniciar con Windows";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.Visible = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // panel9
            // 
            panel9.BorderStyle = BorderStyle.FixedSingle;
            panel9.ForeColor = System.Drawing.SystemColors.ControlDark;
            panel9.Location = new System.Drawing.Point(14, 88);
            panel9.Name = "panel9";
            panel9.Size = new System.Drawing.Size(386, 1);
            panel9.TabIndex = 18;
            // 
            // serviceStatusPanel
            // 
            serviceStatusPanel.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            serviceStatusPanel.Controls.Add(pictureErrorBox);
            serviceStatusPanel.Controls.Add(resetWindowsButton);
            serviceStatusPanel.Location = new System.Drawing.Point(-1, 95);
            serviceStatusPanel.Name = "serviceStatusPanel";
            serviceStatusPanel.Size = new System.Drawing.Size(413, 322);
            serviceStatusPanel.TabIndex = 25;
            // 
            // pictureErrorBox
            // 
            pictureErrorBox.BackgroundImage = Properties.Resources.authentica_alert;
            pictureErrorBox.Location = new System.Drawing.Point(41, 56);
            pictureErrorBox.Name = "pictureErrorBox";
            pictureErrorBox.Size = new System.Drawing.Size(341, 65);
            pictureErrorBox.TabIndex = 2;
            pictureErrorBox.TabStop = false;
            // 
            // resetWindowsButton
            // 
            resetWindowsButton.BackColor = System.Drawing.Color.Transparent;
            resetWindowsButton.BackgroundImage = (System.Drawing.Image)resources.GetObject("resetWindowsButton.BackgroundImage");
            resetWindowsButton.BackgroundImageLayout = ImageLayout.Stretch;
            resetWindowsButton.Cursor = Cursors.Hand;
            resetWindowsButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace;
            resetWindowsButton.FlatAppearance.BorderSize = 0;
            resetWindowsButton.FlatStyle = FlatStyle.Flat;
            resetWindowsButton.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            resetWindowsButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            resetWindowsButton.Location = new System.Drawing.Point(94, 191);
            resetWindowsButton.Name = "resetWindowsButton";
            resetWindowsButton.Size = new System.Drawing.Size(227, 46);
            resetWindowsButton.TabIndex = 1;
            resetWindowsButton.UseVisualStyleBackColor = false;
            resetWindowsButton.Click += resetWindowsButton_Click;
            resetWindowsButton.Enter += resetWindows_Enter;
            resetWindowsButton.Leave += resetWindows_Leave;
            resetWindowsButton.MouseLeave += resetWindows_MouseLeave;
            resetWindowsButton.MouseHover += resetWindows_MouseHover;
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.Transparent;
            button1.BackgroundImage = (System.Drawing.Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            button1.Location = new System.Drawing.Point(57, 115);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(299, 64);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            button1.Enter += button1_Enter;
            button1.Leave += button1_Leave;
            button1.MouseLeave += button1_MouseLeave;
            button1.MouseHover += button1_MouseHover;
            // 
            // settingsButton
            // 
            settingsButton.BackColor = System.Drawing.Color.Transparent;
            settingsButton.BackgroundImage = Properties.Resources.gear_40dp1;
            settingsButton.BackgroundImageLayout = ImageLayout.Stretch;
            settingsButton.Cursor = Cursors.Hand;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.FlatStyle = FlatStyle.Flat;
            settingsButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            settingsButton.Location = new System.Drawing.Point(371, -1);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new System.Drawing.Size(42, 38);
            settingsButton.TabIndex = 24;
            settingsButton.UseVisualStyleBackColor = false;
            settingsButton.Click += button2_Click;
            // 
            // rotatedLabel1
            // 
            rotatedLabel1.BackColor = System.Drawing.Color.Transparent;
            rotatedLabel1.BackgroundColor = System.Drawing.Color.Transparent;
            rotatedLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rotatedLabel1.ForeColor = System.Drawing.SystemColors.Control;
            rotatedLabel1.Location = new System.Drawing.Point(1008, 433);
            rotatedLabel1.Name = "rotatedLabel1";
            rotatedLabel1.RotationAngle = -45F;
            rotatedLabel1.Size = new System.Drawing.Size(80, 82);
            rotatedLabel1.TabIndex = 22;
            rotatedLabel1.Text = "rotatedLabel1";
            // 
            // pictureBox12
            // 
            pictureBox12.BackColor = System.Drawing.Color.Transparent;
            pictureBox12.Location = new System.Drawing.Point(1008, 433);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new System.Drawing.Size(100, 100);
            pictureBox12.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox12.TabIndex = 21;
            pictureBox12.TabStop = false;
            pictureBox12.WaitOnLoad = true;
            pictureBox12.Click += pictureBox11_Click;
            pictureBox12.Paint += pictureBox11_Paint;
            // 
            // pictureBox11
            // 
            pictureBox11.BackColor = System.Drawing.SystemColors.Control;
            pictureBox11.Image = Properties.Resources.ribbon1;
            pictureBox11.Location = new System.Drawing.Point(1008, 433);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new System.Drawing.Size(100, 100);
            pictureBox11.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox11.TabIndex = 21;
            pictureBox11.TabStop = false;
            pictureBox11.Visible = false;
            pictureBox11.WaitOnLoad = true;
            // 
            // labelVersion
            // 
            labelVersion.BackColor = System.Drawing.Color.Silver;
            labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelVersion.Location = new System.Drawing.Point(0, 419);
            labelVersion.Name = "labelVersion";
            labelVersion.Padding = new Padding(5);
            labelVersion.Size = new System.Drawing.Size(413, 32);
            labelVersion.TabIndex = 23;
            labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = System.Drawing.Color.Transparent;
            pictureBox7.Image = (System.Drawing.Image)resources.GetObject("pictureBox7.Image");
            pictureBox7.Location = new System.Drawing.Point(0, -1);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new System.Drawing.Size(413, 99);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 23;
            pictureBox7.TabStop = false;
            // 
            // certButtonPanel
            // 
            certButtonPanel.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            certButtonPanel.Location = new System.Drawing.Point(0, 98);
            certButtonPanel.Name = "certButtonPanel";
            certButtonPanel.Size = new System.Drawing.Size(413, 102);
            certButtonPanel.TabIndex = 26;
            // 
            // Signature
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(413, 449);
            Controls.Add(serviceStatusPanel);
            Controls.Add(mainPanel);
            Controls.Add(button1);
            Controls.Add(settingsButton);
            Controls.Add(pictureBox7);
            Controls.Add(labelVersion);
            Controls.Add(rotatedLabel1);
            Controls.Add(pictureBox12);
            Controls.Add(pictureBox11);
            Controls.Add(certPanel);
            Controls.Add(certButtonPanel);
            Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ForeColor = System.Drawing.Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Signature";
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            Load += Configuración_Load;
            Resize += Form_Hide;
            contextMenuStrip1.ResumeLayout(false);
            certPanel.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            settingsPanel.ResumeLayout(false);
            serviceStatusPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureErrorBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Panel certPanel;
        private Panel panel9;
        private Button button1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel4;
        private PictureBox pictureBox5;
        private Label label6;
        private Panel mainPanel;
        private CheckBox checkBox1;
        private PictureBox pictureBox11;
        private PictureBox pictureBox12;
        private RotatedLabel rotatedLabel1;
        private Label labelVersion;
        private CheckBox ontiCheckBox;
        private Button settingsButton;
        private Panel settingsPanel;
        private Panel serviceStatusPanel;
        private Button resetWindowsButton;
        private PictureBox pictureErrorBox;
        private PictureBox pictureBox7;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Label label3;
        private Label label2;
        private Panel certButtonPanel;
    }
}

