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
            DialogResult result = MessageBox.Show(this, "¿Desea cerrar la aplicación?", "Cerrar", MessageBoxButtons.YesNo);
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
            panel2 = new Panel();
            panel7 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            panel9 = new Panel();
            panel4 = new Panel();
            pictureBox5 = new PictureBox();
            label6 = new Label();
            pictureBox7 = new PictureBox();
            panel3 = new Panel();
            label3 = new Label();
            label2 = new Label();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            button1 = new Button();
            rotatedLabel1 = new RotatedLabel();
            pictureBox12 = new PictureBox();
            pictureBox11 = new PictureBox();
            checkBox1 = new CheckBox();
            label1 = new Label();
            labelVersion = new Label();
            checkBox2 = new CheckBox();
            contextMenuStrip1.SuspendLayout();
            panel2.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
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
            // panel2
            // 
            panel2.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            panel2.Controls.Add(panel7);
            panel2.Controls.Add(pictureBox7);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(button1);
            panel2.Location = new System.Drawing.Point(0, 26);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(413, 410);
            panel2.TabIndex = 6;
            panel2.Paint += panel2_Paint;
            // 
            // panel7
            // 
            panel7.BackColor = System.Drawing.Color.FromArgb(235, 240, 246, 255);
            panel7.Controls.Add(pictureBox2);
            panel7.Controls.Add(pictureBox1);
            panel7.Controls.Add(panel9);
            panel7.Controls.Add(panel4);
            panel7.Location = new System.Drawing.Point(1, 203);
            panel7.Name = "panel7";
            panel7.Size = new System.Drawing.Size(411, 208);
            panel7.TabIndex = 21;
            panel7.Paint += panel7_Paint_2;
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
            // panel9
            // 
            panel9.BorderStyle = BorderStyle.FixedSingle;
            panel9.ForeColor = System.Drawing.SystemColors.ControlDark;
            panel9.Location = new System.Drawing.Point(14, 88);
            panel9.Name = "panel9";
            panel9.Size = new System.Drawing.Size(386, 1);
            panel9.TabIndex = 18;
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
            // pictureBox7
            // 
            pictureBox7.BackColor = System.Drawing.Color.Transparent;
            pictureBox7.Image = (System.Drawing.Image)resources.GetObject("pictureBox7.Image");
            pictureBox7.Location = new System.Drawing.Point(-3, 0);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new System.Drawing.Size(420, 104);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 23;
            pictureBox7.TabStop = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(listView1);
            panel3.Location = new System.Drawing.Point(10, 203);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(390, 199);
            panel3.TabIndex = 7;
            panel3.Visible = false;
            // 
            // label3
            // 
            label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(285, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(105, 24);
            label3.TabIndex = 8;
            label3.Text = "Verificación";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(0, 0);
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
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(390, 198);
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
            button1.Location = new System.Drawing.Point(54, 122);
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
            // checkBox1
            // 
            checkBox1.BackColor = System.Drawing.Color.Silver;
            checkBox1.FlatStyle = FlatStyle.Flat;
            checkBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            checkBox1.ForeColor = System.Drawing.Color.FromArgb(50, 53, 51);
            checkBox1.Location = new System.Drawing.Point(15, 443);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(163, 37);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Iniciar con Windows";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.BackColor = System.Drawing.Color.Silver;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label1.Location = new System.Drawing.Point(0, 438);
            label1.Name = "label1";
            label1.Padding = new Padding(5);
            label1.Size = new System.Drawing.Size(413, 47);
            label1.TabIndex = 2;
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label1.Click += label1_Click;
            // 
            // labelVersion
            // 
            labelVersion.BackColor = System.Drawing.Color.Silver;
            labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelVersion.Location = new System.Drawing.Point(0, 0);
            labelVersion.Name = "labelVersion";
            labelVersion.Padding = new Padding(5);
            labelVersion.Size = new System.Drawing.Size(413, 32);
            labelVersion.TabIndex = 23;
            labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox2
            // 
            checkBox2.BackColor = System.Drawing.Color.Silver;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.FlatStyle = FlatStyle.Flat;
            checkBox2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            checkBox2.ForeColor = System.Drawing.Color.FromArgb(50, 53, 51);
            checkBox2.Location = new System.Drawing.Point(184, 444);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new System.Drawing.Size(94, 37);
            checkBox2.TabIndex = 24;
            checkBox2.Text = "Onti";
            checkBox2.UseVisualStyleBackColor = false;
            checkBox2.Visible = false;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // Signature
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(413, 484);
            Controls.Add(checkBox2);
            Controls.Add(labelVersion);
            Controls.Add(rotatedLabel1);
            Controls.Add(checkBox1);
            Controls.Add(pictureBox12);
            Controls.Add(panel2);
            Controls.Add(label1);
            Controls.Add(pictureBox11);
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
            panel2.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Panel panel2;
        private Panel panel9;
        private Button button1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel4;
        private PictureBox pictureBox5;
        private Label label6;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Panel panel7;
        private Label label2;
        private Label label3;
        private Panel panel3;
        private Label label1;
        private CheckBox checkBox1;
        private PictureBox pictureBox7;
        private PictureBox pictureBox11;
        private PictureBox pictureBox12;
        private RotatedLabel rotatedLabel1;
        private Label labelVersion;
        private CheckBox checkBox2;
    }
}

