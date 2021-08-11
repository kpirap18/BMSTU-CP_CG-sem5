using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;


namespace WinFormsApp2
{
    public class Scene
    {
        private readonly PictureBox _picturebox;
        private readonly Point3D _center;
        private double _pov1, _pov2;
        public double xd;
        public bool flag;
        readonly Point3D _point = new Point3D();

    }
}
