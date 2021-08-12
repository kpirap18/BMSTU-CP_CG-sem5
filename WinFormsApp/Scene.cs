using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp
{
    class Scene
    {
        public Scene(PictureBox pictureBox)
        {
            _flags = true;
            _picture = new PictureBox();
            _picture = pictureBox;
            _picture.BackColor = Color.Black;
            DrawEarth();
            _center = new Point3D(_picture.Width / 2, _picture.Height / 2, 0);
            _house = new House(_picture.Width, _picture.Height, _center, 1, 5);
            _ya = 1;
            Kol = 5;
            _p.X = (_house.S[0].P[2].X + _house.S[2].P[2].X) / 2;
            _p.Y = _house.S[0].P[2].Y + 225;
            _p.Z = 5000;
            Oldy = 0;
            _window = true;
        }

        private double Oldy { get; set; }

        private bool _window;
        private Shadow _s;
        private readonly PictureBox _picture;
        private readonly Point3D _center;
        private double _yan, _ya;
        public double Xd;
        public bool Flag;
        private bool _flags;
        readonly Point3D _p = new Point3D();
        private House _house;
        private Lightning _light;
        private Point3D _l;
        private bool[] _sh;
        public int Kol;
    }

}
