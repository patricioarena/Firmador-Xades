using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Demo
{
    internal class CustomButton : Button
    {
        //Fields
        private int _borderSize = 0;
        private int _borderRadius = 20;
        private Color _borderColor = Color.Silver;
        private Image _icon;

        //Properties
        [Category("Border Size")]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                Invalidate();
            }
        }
        [Category("Border Radius")]
        public int BorderRadius
        {
            get { return _borderRadius; }
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }
        [Category("Border Color")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }
        [Category("Background Color")]
        public Color BackgroundColor
        {
            get { return BackColor; }
            set { BackColor = value; }
        }
        [Category("Text Color")]
        public Color TextColor
        {
            get { return ForeColor; }
            set { ForeColor = value; }
        }
        [Category("Image Icon")]
        public Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        //Constructor
        public CustomButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            BackColor = Color.White;
            ForeColor = Color.White;
            Resize += new EventHandler(Button_Resize);

        }
        private void Button_Resize(object sender, EventArgs e)
        {
            if (_borderRadius > Height)
                _borderRadius = Height;
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
            base.OnPaint(pevent);

            Rectangle rectSurface = ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, _borderSize, _borderSize);
            int smoothSize = 2;

            if (_borderSize > 0)
                smoothSize = _borderSize;

            if (_borderRadius > 2) //Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, _borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, _borderRadius - _borderSize))
                using (Pen penSurface = new Pen(Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(_borderColor, _borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                    Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    //Button border                    
                    if (_borderSize >= 1)
                        //Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);

                }
            }
            else //Normal button
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                //Button surface
                Region = new Region(rectSurface);
                //Button border
                if (_borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(_borderColor, _borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, Width - 1, Height - 1);

                    }
                }
            }

            if (_icon != null)
            {
                int iconX = 5; // Ajustar la posición X según tus necesidades
                int iconY = (Height - _icon.Height) / 2;
                pevent.Graphics.DrawImage(_icon, iconX, iconY);
            }

            DropShadow(pevent.Graphics);
        }

        private void DropShadow(Graphics graphics)
        {
            Color[] shadow = new Color[3];
            shadow[0] = Color.FromArgb(181, 181, 181);
            shadow[1] = Color.FromArgb(195, 195, 195);
            shadow[2] = Color.FromArgb(211, 211, 211);
            Pen pen = new Pen(shadow[0]);
            using (pen)
            {
                foreach (CustomButton customButton in Controls.OfType<CustomButton>())
                {
                    Point pt = customButton.Location;
                    pt.Y += customButton.Height;
                    for (var sp = 0; sp < 3; sp++)
                    {
                        pen.Color = shadow[sp];
                        graphics.DrawLine(pen, pt.X + sp, pt.Y, pt.X + customButton.Width - 1 + sp, pt.Y);
                        graphics.DrawLine(pen, customButton.Right + sp, customButton.Top + sp, customButton.Right + sp, customButton.Bottom + sp);
                        pt.Y++;
                    }
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
