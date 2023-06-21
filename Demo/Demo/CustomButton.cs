using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Demo
{
    internal class CustomButton : Button
    {
        //Fields
        private int borderSize = 0;
        private int borderRadius = 20;
        private Color borderColor = Color.Silver;
        private Image icon;

        //Properties
        [Category("Border Size")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        [Category("Border Radius")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }
        [Category("Border Color")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("Background Color")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }
        [Category("Text Color")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }


        [Category("Image Icon")]
        public Image Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                Invalidate();
            }
        }

        //Constructor
        public CustomButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.White;
            this.ForeColor = Color.White;
            this.Resize += new EventHandler(Button_Resize);

        }
        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }

        //Methods
        private GraphicsPath GetFigurePath(Rectangle rect, float radius) 
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //dropShadow(this, pevent);
            base.OnPaint(pevent);

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;

            if (borderSize > 0)
                smoothSize = borderSize;

            if (borderRadius > 2) //Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                    this.Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    //Button border                    
                    if (borderSize >= 1)
                        //Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                        //dropShadow(this, pevent);
                        //base.OnPaint(pevent);
                }
            }
            else //Normal button
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                //Button surface
                this.Region = new Region(rectSurface);
                //Button border
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                        //dropShadow(this, pevent);
                        //base.OnPaint(pevent);
                    }
                }
            }
            // Dibujar el icono si está definido
            if (icon != null)
            {
                int iconX = 5; // Ajustar la posición X según tus necesidades
                int iconY = (Height - icon.Height) / 2;
                pevent.Graphics.DrawImage(icon, iconX, iconY);
            }
        }

        //private void dropShadow(object sender, PaintEventArgs e)
        //{
        //    RJButton button = (RJButton)sender;
        //    Color[] shadow = new Color[3];
        //    //shadow[0] = Color.FromArgb(181, 181, 181);
        //    //shadow[1] = Color.FromArgb(195, 195, 195);
        //    //shadow[2] = Color.FromArgb(211, 211, 211);
        //    shadow[0] = Color.FromArgb(0, 0, 255); // Azul
        //    shadow[1] = Color.FromArgb(0, 0, 195); // Azul más oscuro
        //    shadow[2] = Color.FromArgb(0, 0, 135); // Azul aún más oscuro
        //    Pen pen = new Pen(shadow[0]);
        //    using (pen)
        //    {
        //        //foreach (RJButton p in button.Controls.OfType<RJButton>())
        //        //{
        //            Point pt = button.Location;
        //            pt.Y += button.Height;
        //            for (var sp = 0; sp < 3; sp++)
        //            {
        //                pen.Color = shadow[sp];
        //                e.Graphics.DrawLine(pen, pt.X + sp, pt.Y, pt.X +button.Width - 1 + sp, pt.Y); 
        //                e.Graphics.DrawLine(pen, button.Right + sp, button.Top + sp, button.Right + sp, button.Bottom + sp);
        //                pt.Y++;
        //            }
        //        //}
        //    }
        //}

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
