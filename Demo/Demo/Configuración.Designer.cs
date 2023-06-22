using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
            DialogResult result = MessageBox.Show(this, "¿Desea minimizar al área de notificaciones?", "Minimizar", MessageBoxButtons.YesNo);
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Signature));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel5 = new System.Windows.Forms.Panel();
            this.customButton1 = new Demo.CustomButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.customButton6 = new Demo.CustomButton();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.customButton4 = new Demo.CustomButton();
            this.customButton3 = new Demo.CustomButton();
            this.customButton2 = new Demo.CustomButton();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.rotatedLabel2 = new Demo.RotatedLabel();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rotatedLabel1 = new Demo.RotatedLabel();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.customButton7 = new Demo.CustomButton();
            this.panel10 = new System.Windows.Forms.Panel();
            this.customButton5 = new Demo.CustomButton();
            this.panel8 = new System.Windows.Forms.Panel();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Form_Show);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.showToolStripMenuItem.Text = "Abrir";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1070, 674);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "Firmar PDF";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(966, 673);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "Active SSL";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(904, 680);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "SSL State";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(823, 674);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "SSL Check";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(1202, 674);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 34);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fiscalia de Estado de la Provincia de Buenos Aires";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.checkBox1.Location = new System.Drawing.Point(57, 395);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(163, 37);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Iniciar con Windows";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "pdf";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Archivos PDF|*.pdf";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "pdf";
            this.saveFileDialog1.FileName = "8b0ee826-94d5-4625-b900-054ae12e2570";
            this.saveFileDialog1.Filter = "Archivos PDF|*.pdf";
            this.saveFileDialog1.Title = "Guardar archivo PDF";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.panel5.Controls.Add(this.customButton1);
            this.panel5.Controls.Add(this.rotatedLabel2);
            this.panel5.Controls.Add(this.pictureBox14);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.pictureBox13);
            this.panel5.Controls.Add(this.pictureBox10);
            this.panel5.Controls.Add(this.pictureBox9);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.customButton4);
            this.panel5.Controls.Add(this.customButton3);
            this.panel5.Controls.Add(this.customButton2);
            this.panel5.Controls.Add(this.checkBox1);
            this.panel5.Controls.Add(this.panel11);
            this.panel5.Controls.Add(this.panel12);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Location = new System.Drawing.Point(12, 48);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(411, 530);
            this.panel5.TabIndex = 19;
            this.panel5.Visible = false;
            // 
            // customButton1
            // 
            this.customButton1.BackColor = System.Drawing.Color.GhostWhite;
            this.customButton1.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.customButton1.BorderColor = System.Drawing.Color.Silver;
            this.customButton1.BorderRadius = 5;
            this.customButton1.BorderSize = 1;
            this.customButton1.FlatAppearance.BorderSize = 0;
            this.customButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton1.Icon = null;
            this.customButton1.Image = global::Demo.Properties.Resources.arrow_right_from_bracket_20dp__1_;
            this.customButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButton1.Location = new System.Drawing.Point(294, 489);
            this.customButton1.Name = "customButton1";
            this.customButton1.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.customButton1.Size = new System.Drawing.Size(105, 33);
            this.customButton1.TabIndex = 19;
            this.customButton1.Text = "Salir";
            this.customButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.customButton1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton1.UseVisualStyleBackColor = false;
            this.customButton1.Click += new System.EventHandler(this.customButton1_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel6.Controls.Add(this.customButton6);
            this.panel6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panel6.Location = new System.Drawing.Point(357, 38);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(54, 51);
            this.panel6.TabIndex = 21;
            // 
            // customButton6
            // 
            this.customButton6.BackColor = System.Drawing.Color.Transparent;
            this.customButton6.BackgroundColor = System.Drawing.Color.Transparent;
            this.customButton6.BorderColor = System.Drawing.Color.Silver;
            this.customButton6.BorderRadius = 5;
            this.customButton6.BorderSize = 1;
            this.customButton6.FlatAppearance.BorderSize = 0;
            this.customButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton6.Icon = null;
            this.customButton6.Image = global::Demo.Properties.Resources.xmark_30dp;
            this.customButton6.Location = new System.Drawing.Point(-1, 0);
            this.customButton6.Name = "customButton6";
            this.customButton6.Size = new System.Drawing.Size(54, 51);
            this.customButton6.TabIndex = 22;
            this.customButton6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.customButton6.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton6.UseVisualStyleBackColor = false;
            this.customButton6.Click += new System.EventHandler(this.customButton6_Click);
            // 
            // pictureBox10
            // 
            this.pictureBox10.Image = global::Demo.Properties.Resources.shield_xmark_20dp;
            this.pictureBox10.Location = new System.Drawing.Point(168, 136);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(41, 33);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox10.TabIndex = 24;
            this.pictureBox10.TabStop = false;
            this.pictureBox10.Visible = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Image = global::Demo.Properties.Resources.shield_check_20dp;
            this.pictureBox9.Location = new System.Drawing.Point(168, 136);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(41, 33);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox9.TabIndex = 23;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.Visible = false;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(168, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 33);
            this.label13.TabIndex = 20;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customButton4
            // 
            this.customButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.customButton4.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.customButton4.BorderColor = System.Drawing.Color.Silver;
            this.customButton4.BorderRadius = 5;
            this.customButton4.BorderSize = 1;
            this.customButton4.FlatAppearance.BorderSize = 0;
            this.customButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.customButton4.Icon = null;
            this.customButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButton4.Location = new System.Drawing.Point(251, 136);
            this.customButton4.Name = "customButton4";
            this.customButton4.Size = new System.Drawing.Size(105, 33);
            this.customButton4.TabIndex = 22;
            this.customButton4.Text = "Firmar PDF";
            this.customButton4.TextColor = System.Drawing.SystemColors.ControlLightLight;
            this.customButton4.UseVisualStyleBackColor = false;
            this.customButton4.Click += new System.EventHandler(this.customButton4_Click);
            // 
            // customButton3
            // 
            this.customButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.customButton3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.customButton3.BorderColor = System.Drawing.Color.Silver;
            this.customButton3.BorderRadius = 5;
            this.customButton3.BorderSize = 1;
            this.customButton3.FlatAppearance.BorderSize = 0;
            this.customButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.customButton3.Icon = null;
            this.customButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButton3.Location = new System.Drawing.Point(55, 210);
            this.customButton3.Name = "customButton3";
            this.customButton3.Size = new System.Drawing.Size(105, 33);
            this.customButton3.TabIndex = 21;
            this.customButton3.Text = "Active SSL";
            this.customButton3.TextColor = System.Drawing.SystemColors.ControlLightLight;
            this.customButton3.UseVisualStyleBackColor = false;
            this.customButton3.Click += new System.EventHandler(this.customButton3_Click);
            // 
            // customButton2
            // 
            this.customButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.customButton2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.customButton2.BorderColor = System.Drawing.Color.Silver;
            this.customButton2.BorderRadius = 5;
            this.customButton2.BorderSize = 1;
            this.customButton2.FlatAppearance.BorderSize = 0;
            this.customButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.customButton2.Icon = null;
            this.customButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButton2.Location = new System.Drawing.Point(57, 136);
            this.customButton2.Name = "customButton2";
            this.customButton2.Size = new System.Drawing.Size(105, 33);
            this.customButton2.TabIndex = 20;
            this.customButton2.Text = "SSL Check";
            this.customButton2.TextColor = System.Drawing.SystemColors.ControlLightLight;
            this.customButton2.UseVisualStyleBackColor = false;
            this.customButton2.Click += new System.EventHandler(this.customButton2_Click);
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.panel11.Location = new System.Drawing.Point(13, 352);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(386, 1);
            this.panel11.TabIndex = 18;
            this.panel11.Paint += new System.Windows.Forms.PaintEventHandler(this.panel11_Paint);
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.pictureBox8);
            this.panel12.Controls.Add(this.label11);
            this.panel12.Controls.Add(this.label12);
            this.panel12.Location = new System.Drawing.Point(57, 38);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(299, 50);
            this.panel12.TabIndex = 17;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.Image = global::Demo.Properties.Resources.cucarda;
            this.pictureBox8.Location = new System.Drawing.Point(68, 5);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(35, 42);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 8;
            this.pictureBox8.TabStop = false;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.Location = new System.Drawing.Point(164, 5);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(54, 40);
            this.label11.TabIndex = 7;
            this.label11.Text = "Fisc";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.label12.Location = new System.Drawing.Point(99, 5);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label12.Size = new System.Drawing.Size(80, 40);
            this.label12.TabIndex = 7;
            this.label12.Text = "certi";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Silver;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.Location = new System.Drawing.Point(0, 483);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(5);
            this.label14.Size = new System.Drawing.Size(411, 47);
            this.label14.TabIndex = 2;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rotatedLabel2
            // 
            this.rotatedLabel2.BackColor = System.Drawing.Color.Transparent;
            this.rotatedLabel2.BackgroundColor = System.Drawing.Color.Transparent;
            this.rotatedLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotatedLabel2.ForeColor = System.Drawing.SystemColors.Control;
            this.rotatedLabel2.Location = new System.Drawing.Point(1, 1);
            this.rotatedLabel2.Name = "rotatedLabel2";
            this.rotatedLabel2.RotationAngle = -45F;
            this.rotatedLabel2.Size = new System.Drawing.Size(80, 82);
            this.rotatedLabel2.TabIndex = 24;
            this.rotatedLabel2.Text = "rotatedLabel2";
            // 
            // pictureBox14
            // 
            this.pictureBox14.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox14.Location = new System.Drawing.Point(0, 0);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(100, 100);
            this.pictureBox14.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox14.TabIndex = 23;
            this.pictureBox14.TabStop = false;
            this.pictureBox14.WaitOnLoad = true;
            this.pictureBox14.Click += new System.EventHandler(this.pictureBox11_Click);
            this.pictureBox14.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox11_Paint);
            // 
            // pictureBox13
            // 
            this.pictureBox13.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox13.Image = global::Demo.Properties.Resources.ribbon1;
            this.pictureBox13.Location = new System.Drawing.Point(0, 0);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(100, 100);
            this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox13.TabIndex = 23;
            this.pictureBox13.TabStop = false;
            this.pictureBox13.Visible = false;
            this.pictureBox13.WaitOnLoad = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.rotatedLabel1);
            this.panel2.Controls.Add(this.pictureBox12);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.customButton7);
            this.panel2.Controls.Add(this.panel10);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.pictureBox11);
            this.panel2.Location = new System.Drawing.Point(12, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(411, 530);
            this.panel2.TabIndex = 6;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // rotatedLabel1
            // 
            this.rotatedLabel1.BackColor = System.Drawing.Color.Transparent;
            this.rotatedLabel1.BackgroundColor = System.Drawing.Color.Transparent;
            this.rotatedLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rotatedLabel1.ForeColor = System.Drawing.SystemColors.Control;
            this.rotatedLabel1.Location = new System.Drawing.Point(3, 1);
            this.rotatedLabel1.Name = "rotatedLabel1";
            this.rotatedLabel1.RotationAngle = -45F;
            this.rotatedLabel1.Size = new System.Drawing.Size(80, 82);
            this.rotatedLabel1.TabIndex = 22;
            this.rotatedLabel1.Text = "rotatedLabel1";
            // 
            // pictureBox12
            // 
            this.pictureBox12.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox12.Location = new System.Drawing.Point(0, 0);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(100, 100);
            this.pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox12.TabIndex = 21;
            this.pictureBox12.TabStop = false;
            this.pictureBox12.WaitOnLoad = true;
            this.pictureBox12.Click += new System.EventHandler(this.pictureBox11_Click);
            this.pictureBox12.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox11_Paint);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.listView1);
            this.panel3.Location = new System.Drawing.Point(10, 207);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(390, 270);
            this.panel3.TabIndex = 7;
            this.panel3.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(285, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Verificación";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Certificados";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(390, 264);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListView_KeyDown);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListView1MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Certificados";
            this.columnHeader1.Width = 273;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Verificación";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 90;
            // 
            // customButton7
            // 
            this.customButton7.BackColor = System.Drawing.Color.GhostWhite;
            this.customButton7.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.customButton7.BorderColor = System.Drawing.Color.Silver;
            this.customButton7.BorderRadius = 5;
            this.customButton7.BorderSize = 1;
            this.customButton7.FlatAppearance.BorderSize = 0;
            this.customButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton7.Icon = null;
            this.customButton7.Image = global::Demo.Properties.Resources.arrow_right_from_bracket_20dp__1_;
            this.customButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButton7.Location = new System.Drawing.Point(293, 489);
            this.customButton7.Name = "customButton7";
            this.customButton7.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.customButton7.Size = new System.Drawing.Size(105, 33);
            this.customButton7.TabIndex = 20;
            this.customButton7.Text = "Salir";
            this.customButton7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.customButton7.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton7.UseVisualStyleBackColor = false;
            this.customButton7.Click += new System.EventHandler(this.customButton7_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Transparent;
            this.panel10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel10.Controls.Add(this.customButton5);
            this.panel10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panel10.Location = new System.Drawing.Point(357, 38);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(54, 51);
            this.panel10.TabIndex = 19;
            this.panel10.Paint += new System.Windows.Forms.PaintEventHandler(this.panel10_Paint);
            // 
            // customButton5
            // 
            this.customButton5.BackColor = System.Drawing.Color.Transparent;
            this.customButton5.BackgroundColor = System.Drawing.Color.Transparent;
            this.customButton5.BorderColor = System.Drawing.Color.Silver;
            this.customButton5.BorderRadius = 5;
            this.customButton5.BorderSize = 1;
            this.customButton5.FlatAppearance.BorderSize = 0;
            this.customButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton5.Icon = null;
            this.customButton5.Image = global::Demo.Properties.Resources.gear_30dp;
            this.customButton5.Location = new System.Drawing.Point(-1, 0);
            this.customButton5.Name = "customButton5";
            this.customButton5.Size = new System.Drawing.Size(54, 51);
            this.customButton5.TabIndex = 20;
            this.customButton5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.customButton5.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.customButton5.UseVisualStyleBackColor = false;
            this.customButton5.Click += new System.EventHandler(this.customButton5_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.pictureBox7);
            this.panel8.Controls.Add(this.label10);
            this.panel8.Controls.Add(this.label9);
            this.panel8.Location = new System.Drawing.Point(57, 38);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(299, 50);
            this.panel8.TabIndex = 17;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.pictureBox7.Image = global::Demo.Properties.Resources.cucarda;
            this.pictureBox7.Location = new System.Drawing.Point(68, 5);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(35, 42);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 8;
            this.pictureBox7.TabStop = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.Location = new System.Drawing.Point(164, 5);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label10.Size = new System.Drawing.Size(70, 40);
            this.label10.TabIndex = 7;
            this.label10.Text = "Fisc";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(83)))), ((int)(((byte)(150)))));
            this.label9.Location = new System.Drawing.Point(98, 5);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label9.Size = new System.Drawing.Size(83, 40);
            this.label9.TabIndex = 7;
            this.label9.Text = "certi";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::Demo.Properties.Resources.boton1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Default;
            this.button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(57, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(299, 64);
            this.button1.TabIndex = 0;
            this.button1.Text = "Certificados";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.Enter += new System.EventHandler(this.button1_Enter);
            this.button1.Leave += new System.EventHandler(this.button1_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 483);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(411, 47);
            this.label1.TabIndex = 2;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox11
            // 
            this.pictureBox11.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox11.Image = global::Demo.Properties.Resources.ribbon1;
            this.pictureBox11.Location = new System.Drawing.Point(0, 0);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(100, 100);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox11.TabIndex = 21;
            this.pictureBox11.TabStop = false;
            this.pictureBox11.Visible = false;
            this.pictureBox11.WaitOnLoad = true;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.panel7.Controls.Add(this.pictureBox4);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.pictureBox2);
            this.panel7.Controls.Add(this.pictureBox1);
            this.panel7.Controls.Add(this.panel9);
            this.panel7.Controls.Add(this.panel4);
            this.panel7.Location = new System.Drawing.Point(12, 255);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(411, 273);
            this.panel7.TabIndex = 21;
            this.panel7.Paint += new System.Windows.Forms.PaintEventHandler(this.panel7_Paint_2);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = global::Demo.Properties.Resources.map_18dp;
            this.pictureBox4.Location = new System.Drawing.Point(39, 157);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(18, 18);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 8;
            this.pictureBox4.TabStop = false;
            // 
            // label5
            // 
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(57, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(339, 34);
            this.label5.TabIndex = 4;
            this.label5.Text = "Avenida 1 esq 60 #1342 - La Plata, Buenos Aires";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            this.label5.MouseLeave += new System.EventHandler(this.label5_MouseLeave);
            this.label5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label5_MouseMove);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Demo.Properties.Resources.logo_cife_color_v2;
            this.pictureBox2.Location = new System.Drawing.Point(292, 17);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(105, 41);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Demo.Properties.Resources.logo_fiscalia;
            this.pictureBox1.Location = new System.Drawing.Point(14, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(192, 41);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.panel9.Location = new System.Drawing.Point(13, 102);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(386, 1);
            this.panel9.TabIndex = 18;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.pictureBox6);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.pictureBox5);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Location = new System.Drawing.Point(14, 144);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(385, 78);
            this.panel4.TabIndex = 12;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.Image = global::Demo.Properties.Resources.telephone_18dp;
            this.pictureBox6.Location = new System.Drawing.Point(54, 39);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(18, 18);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 10;
            this.pictureBox6.TabStop = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(72, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 18);
            this.label7.TabIndex = 5;
            this.label7.Text = "0221 429-6294";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.Image = global::Demo.Properties.Resources.globe_18dp;
            this.pictureBox5.Location = new System.Drawing.Point(207, 40);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(18, 18);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 9;
            this.pictureBox5.TabStop = false;
            // 
            // label6
            // 
            this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(53)))), ((int)(((byte)(51)))));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(228, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 18);
            this.label6.TabIndex = 6;
            this.label6.Text = "www.fepba.gov.ar";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            this.label6.MouseLeave += new System.EventHandler(this.label6_MouseLeave);
            this.label6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label6_MouseMove);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Demo.Properties.Resources.baseline_business_black_18dp;
            this.pictureBox3.Location = new System.Drawing.Point(1178, 679);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(18, 18);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // Signature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1388, 1061);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Signature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Configuración_Load);
            this.Resize += new System.EventHandler(this.Form_Hide);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.CheckBox checkBox1;
        private Label label4;
        private PictureBox pictureBox3;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private Button button2;
        private Label label8;
        private Button button4;
        private Button button5;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private CustomButton customButton1;
        private Panel panel5;
        private Panel panel11;
        private Panel panel12;
        private PictureBox pictureBox8;
        private Label label11;
        private Label label12;
        private Label label14;
        private CustomButton customButton4;
        private CustomButton customButton3;
        private CustomButton customButton2;
        private Label label13;
        private PictureBox pictureBox9;
        private PictureBox pictureBox10;
        private Panel panel6;
        private CustomButton customButton6;
        private Panel panel2;
        private CustomButton customButton7;
        private Panel panel10;
        private CustomButton customButton5;
        private Panel panel9;
        private Panel panel8;
        private PictureBox pictureBox7;
        private Label label10;
        private Label label9;
        private Label label5;
        private Button button1;
        private Label label1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox4;
        private PictureBox pictureBox2;
        private Panel panel4;
        private PictureBox pictureBox6;
        private Label label7;
        private PictureBox pictureBox5;
        private Label label6;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Panel panel7;
        private Label label2;
        private Label label3;
        private Panel panel3;
        private PictureBox pictureBox11;
        private PictureBox pictureBox12;
        private RotatedLabel rotatedLabel1;
        private RotatedLabel rotatedLabel2;
        private PictureBox pictureBox14;
        private PictureBox pictureBox13;
    }
}

