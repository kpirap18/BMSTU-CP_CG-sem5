using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public enum TextureType { Sky, Ground, House };
    class Texture
    {
        protected TextureType Tt;
        protected TextureBrush Tb;
    }
}
