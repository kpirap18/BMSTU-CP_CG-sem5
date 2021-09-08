using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    class VectorTexture: Texture
    {
        public VectorTexture(TextureType textureType, PointF[] pol)
        {
            Tt = textureType;
            _ = Image.FromFile("2.jpg");
            B = new TextureBrush(_);
            _polygon = new PointF[4];
            pol.CopyTo(_polygon, 0);
            var v1 = new PointF(pol[2].X - pol[1].X, pol[2].Y - pol[1].Y);
            var v2 = new PointF(pol[0].X - pol[1].X, pol[0].Y - pol[1].Y);
            double a =
                Math.Acos((v1.X * v2.X + v1.Y * v2.Y) / Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y) / Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y)) /
                Math.PI * 180;
            B.RotateTransform((float)(90 - a), MatrixOrder.Prepend);
            B.TranslateTransform(pol[0].X, pol[0].Y);
            B.ScaleTransform(Math.Abs(pol[1].X - pol[0].X) / _.Width / 3, Math.Abs(pol[3].Y - pol[1].Y) / _.Height / 3);
            
        }
        public void DrawTexture(Graphics g)
        {
            g.FillPolygon(B, _polygon);
            g.FillPolygon(new SolidBrush(Color.FromArgb(150, Color.Black)), _polygon);
        }

        private readonly Image _;
        private readonly PointF[] _polygon;
    }
}
