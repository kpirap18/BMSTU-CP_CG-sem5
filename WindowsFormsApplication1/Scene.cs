﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
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
        private void DrawSky()
        {
            var bmp = new Bitmap(_picture.Image);
            Graphics g = Graphics.FromImage(bmp);
            var t = new SimpleTexture(TextureType.Sky, new Point(0, 0),
                                      new Point(_picture.Width, _picture.Height / 2 + 30));
            t.DrawTexture(g, (int)_ya);
            var p = new Point3D(_light.Model[0].X, _light.Model[0].Y, _light.Model[0].Z);
            Transform.RotateY(_center, p, _ya);
            double kof = (p.Z) / 400 * 0.5 + 1;
            Transform.Scale(_house.La[1], p, kof);
            _light.DrawArea(g, p);
            _picture.Image = bmp;
        }
        private void DrawHouse()
        {
            _house.DrawHouse(_picture, _light, new Point3D((float)_ya, (float)_yan, 0), _sh, _p, _window);
        }
        private void DrawShadow()
        {
            if (_flags)
            {
                var p = new Point3D(_l.X, _l.Y + 100, _l.Z);
                Transform.RotateY(_center, p, _ya);
                _s = new Shadow(_center, p, _house, _yan);
            }
            else
            {
                _flags = true;
            }
            var bmp = new Bitmap(_picture.Image);
            Graphics g = Graphics.FromImage(bmp);
            _s.DrawShadow(g);
            _picture.Image = bmp;
        }
        private void DrawEarth()
        {
            var bmp1 = new Bitmap(_picture.Width, _picture.Height);
            Graphics g = Graphics.FromImage(bmp1);
            var t = new SimpleTexture(TextureType.Ground, new Point(0, _picture.Height / 2 + 30),
                                      new Point(_picture.Width, _picture.Height));
            t.DrawTexture(g, (int)_ya);
            _picture.Image = bmp1;
        }
        private void DrawLightning(Lightning light)
        {
            light.DrawLightning(_picture);
        }
        public void Timer1()
        {
            _light = new Lightning(_picture.Height, _picture.Width, _house.La[1]);
            init_l();
            var p = new Point3D(_light.Model[0].X, 400, _light.Model[0].Z);
            Transform.RotateY(_center, p, _ya);
            _sh = _house.InShadow2(p);
            DrawScene();
        }
        public void Timer2()
        {
            var r = new Random();
            for (int i = 0; i < 4; i++)
            {
                int k = r.Next(0, 2*Kol-1);
                _house.W[i][k].Light = Math.Abs(_house.W[i][k].Light - 1);
                k = r.Next(0, 2 * Kol - 1);
                _house.W[i][k].Light = Math.Abs(_house.W[i][k].Light - 1);
            }
            _flags = false;
            DrawScene();
        }
        public void LightOn()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2 * Kol; j++)
                {
                    _house.W[i][j].Light = 1;
                }
            }
            _flags = false;
            DrawScene();
        }
        public void LightOff()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2*Kol; j++)
                {
                    _house.W[i][j].Light = 0;
                }
            }
            _flags = false;
            DrawScene();
        }
        public void Click()
        {
            _light = new Lightning(_picture.Height, _picture.Width, _house.La[1]);
            init_l();
            var p = new Point3D(_light.Model[0].X, 400, _light.Model[0].Z);
            Transform.RotateY(_center, p, _ya);
            _sh = _house.InShadow2(p);
            DrawScene();
        }
        public void Move(Point e)
        {
            bool fy = false;
            if (Math.Abs(e.X - Xd) == 30)
            {
                if (Xd > e.X)
                {
                    _yan = -15;
                }
                else
                {
                    _yan = 15;
                }
                Xd = e.X;
                fy = true;
            }
            if (fy)
            {
                _ya += _yan;
            }
            if (fy)
            {
                init_l();
                Oldy = -_yan;
                Transform.RotateY(_center, _l, -_yan);
                DrawScene();
                _yan = 0;
            }
        }
        private void Trace()
        {
            var r = new RayTracer();
            var model = new List<Point3D>();
            var subModels = new List<Point3D>();
            var ll = new Lightning(_light);
            ll.RotateLightning(_center, new Point3D(0, (float)_ya, 0));
            double kof = (ll.Model[0].Z) / 400 * 0.5 + 1;
            ll.ScaleLightning(_house.La[1], kof);
            foreach (Point3D t in ll.Model)
            {
                var ray = new Ray(new Point3D(t.X - _p.X, t.Y - _p.Y, t.Z - _p.Z), new Point3D(_p.X, _p.Y, _p.Z));
                Trace c = r.Trace(ray, _house, ll);
                if (c.Dist == 1)
                    if (c.I == 1)
                        model.Add(c.Per);
                    else
                        subModels.Add(c.Per);
            }

            foreach (List<Point3D> t in ll.SubModels)
            {
                foreach (Point3D t1 in t)
                {
                    var ray = new Ray(new Point3D(t1.X - _p.X, t1.Y - _p.Y, t1.Z - _p.Z), new Point3D(_p.X, _p.Y, _p.Z));
                    Trace c = r.Trace(ray, _house, ll);
                    if (c.Dist == 1)
                        if (c.I == 1)
                            model.Add(c.Per);
                        else
                            subModels.Add(c.Per);
                }
            }
            var l = new Lightning(model, subModels);
            DrawLightning(l);
        }
        private void init_l()
        {
            _l = new Point3D((_light.Model[0].X + _light.Model[_light.Model.Count - 1].X) / 2, _light.Model[0].Y,
                (_light.Model[0].Z + _light.Model[_light.Model.Count - 1].Z) / 2);
        }
        public void Button(bool e)
        {
            if (e)
            {
                _yan = 2;
                _ya += _yan;
                Oldy = _yan;
                init_l();
                Transform.RotateY(_center, _l, -_yan);
                DrawScene();
            }
            if (!e)
            {
                _yan = -2;
                _ya += _yan;
                Oldy = _yan;
                init_l();
                Transform.RotateY(_center, _l, -_yan);
                DrawScene();
            }
            _yan = 0;
        }
        private void DrawScene()
        {
            DrawEarth();
            DrawShadow();
            DrawSky();
            DrawHouse();
            Trace();
        }
        public void WindowsChange()
        {
            _window = !_window;
            DrawScene();
        }
        public void FloarChange()
        {
            _house = new House(_picture.Width, _picture.Height, _center, (int)_ya, Kol);
            _light = new Lightning(_picture.Height, _picture.Width, _house.La[1]);
            init_l();
            var p = new Point3D(_light.Model[0].X, 400, _light.Model[0].Z);
            Transform.RotateY(_center, p, _ya);
            _sh = _house.InShadow2(p);
            DrawScene();
        }

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