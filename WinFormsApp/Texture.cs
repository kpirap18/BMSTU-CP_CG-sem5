using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinFormsApp
{
    public enum TextureType { Sky, Ground, ETL };
    class Texture
    {
        protected TextureType Tt;
        protected TextureBrush Tb;
    }
}
