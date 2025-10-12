using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Demo
{
    internal class RotatedLabel : Control
    {

        private float rotationAngle = -45f;
        private PictureBox pictureBox;
        private Color backgroundColor = Color.Transparent;

        public RotatedLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            AutoSize = false;
            BackColor = Color.Transparent;
            UpdateStyles();
        }

        public float RotationAngle
        {
            get { return rotationAngle; }
            set
            {
                rotationAngle = value;
                Invalidate();
            }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                Invalidate();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            // Obtener el color del componente que está detrás
            Color backgroundColor = Parent.BackColor;

            // Dibujar el fondo del RotatedLabel utilizando el color del componente de fondo
            using (SolidBrush brush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush backgroundBrush = new SolidBrush(backgroundColor))
            using (Brush textBrush = new SolidBrush(ForeColor))
            {
                SizeF textSize = e.Graphics.MeasureString(Text, Font);
                PointF center = new PointF(ClientSize.Width / 2f, ClientSize.Height / 2f);

                g.TranslateTransform(center.X, center.Y);
                g.RotateTransform(rotationAngle);
                g.TranslateTransform(-center.X, -center.Y);
                g.DrawString(Text, Font, textBrush, center.X - (textSize.Width / 2f), center.Y - (textSize.Height / 2f));
                g.ResetTransform();
            }
        }
    }
}
