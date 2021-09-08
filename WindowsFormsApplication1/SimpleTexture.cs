using System.Drawing;

namespace WindowsFormsApplication1
{
    class SimpleTexture: Texture
    {
        public SimpleTexture(TextureType textureType, Point p1, Point p2)
        {
            Tt = textureType;
            _region = new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
            switch (Tt)
            {
                case TextureType.Sky:
                    B = new TextureBrush(Image.FromFile("33.jpg"));
                    B.ScaleTransform(0.7F, 0.7F);
                    break;
                case TextureType.Ground:
                    B = new TextureBrush(Image.FromFile("11.jpg"));
                    B.ScaleTransform(0.02F, 0.02F);
                    break;
            }
        }
        public void DrawTexture(Graphics g, int angle)
        {
            B.TranslateTransform(-4*angle, 0);
            g.FillRectangle(B, _region);
            B.TranslateTransform(4*angle, 0);
        }

        private readonly Rectangle _region;
    }
}
