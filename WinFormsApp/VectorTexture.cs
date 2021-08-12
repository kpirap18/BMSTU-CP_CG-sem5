using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinFormsApp
{
    class VectorTexture: Texture
    {
        public VectorTexture(TextureType textype, PointF[] pol)
        {
            Tt = textype;
            _im = Image.FromFile("");

            Tb = new TextureBrush(_im);
            pol.CopyTo(_pol, 0);

            var v_1 = new PointF(pol[2].X - pol[1].X, pol[2].Y - pol[1].Y);
            var v_2 = new PointF(pol[0].X - pol[1].X, pol[0].Y - pol[1].Y);

            double a = Math.Acos((v_1.X * v_2.X + v_1.Y * v_2.Y) /
                Math.Sqrt(v_1.X * v_1.X + v_1.Y * v_1.Y) /
                Math.Sqrt(v_2.X * v_2.X + v_2.Y * v_2.Y)) / Math.PI * 180;

            Tb.RotateTransform((float)(90 - a), MatrixOrder.Prepend);
            Tb.TranslateTransform(pol[0].X, pol[0].Y);
            Tb.ScaleTransform(Math.Abs(pol[1].X - pol[0].X) / _im.Width / 3,
                              Math.Abs(pol[3].Y - pol[1].Y) / _im.Height / 3); 
        }

        public void DrawTexture(Graphics gr)
        {
            gr.FillPolygon(Tb, _pol);
            gr.FillPolygon(new SolidBrush(Color.FromArgb(150, Color.Black)), _pol);
        }


        private readonly Image _im;
        private readonly PointF[] _pol;
    }
}
