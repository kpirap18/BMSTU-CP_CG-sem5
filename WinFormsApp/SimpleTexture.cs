using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinFormsApp
{
    class SimpleTexture : Texture
    {
        public SimpleTexture(TextureType textureType, Point p1, Point p2)
        {
            Tt = textureType;
            _reg = new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            switch (Tt)
            {
                case TextureType.Sky:
                    Tb = new TextureBrush(Image.FromFile("2.jpg"));
                    Tb.ScaleTransform(0.7F, 0.7F);
                    break;
                case TextureType.Ground:
                    Tb = new TextureBrush(Image.FromFile("1.jpg"));
                    Tb.ScaleTransform(0.02F, 0.02F);
                    break;
            }
        }
        public void DrawTexture(Graphics g, int angle)
        {
            Tb.TranslateTransform(-4 * angle, 0);
            g.FillRectangle(Tb, _reg);
            Tb.TranslateTransform(4 * angle, 0);
        }

        private readonly Rectangle _reg;
    }
}
