using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
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
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _scene.Xd = e.X;
            _scene.Flag = true;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled && _scene.Flag)
            {
                _scene.Move(new Point(e.X, e.Y));
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _scene.Flag = false;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _scene.Click();            
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                _scene.Button(true);
            }
            if (e.KeyCode == Keys.Left)
            {
                _scene.Button(false);
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
            _scene.WindowsChange();
        }
        private void myTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 49 || e.KeyChar > 57))
                e.Handled = true;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            _scene.Kol = int.Parse(toolStripTextBox1.Text);
            _scene.FloarChange();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
