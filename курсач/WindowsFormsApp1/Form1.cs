using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly Scene _scene;

        public Form1()
        {
            InitializeComponent();
            _scene = new Scene(pictureBox1);
            _scene.Click();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            _scene.Timer1();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            timer2.Enabled = true;
            _scene.withpause = false;
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            System.Console.WriteLine("YYYEEESSS");
            _scene.Xd = e.X;
            _scene.Flag = true;
            _scene.Button(1);

            // pictureBox1.Focus();
            // pictureBox1.Invalidate();
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled && _scene.Flag)
            {
                _scene.Move(new Point(e.X, e.Y));
            }
            _scene.Button(1);
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _scene.Flag = false;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _scene.withpause = true;
            this.timer2.Interval = int.Parse(toolStripTextBox3.Text);
            _scene.Click();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                _scene.Button(1);
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                _scene.Button(2);
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.A)
            {
                _scene.Button(3);
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            _scene.Timer2();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            _scene.LightOn();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _scene.LightOff();
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }
        private void myTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 49 || e.KeyChar > 57))
                e.Handled = true;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            _scene.Kol = int.Parse(toolStripTextBox1.Text);
            _scene.FLoarChange();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            _scene.LightOn();
        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            _scene.LightOff();
        }

        private void toolStripLabel1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
