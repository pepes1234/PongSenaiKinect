using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontendDesktop
{
    public partial class Form2 : Form
    {
        Random rand = new Random();
        Bitmap bmp = null;
        Graphics g = null;
        World world = new World();

        ParticleBar b1;
        ParticleBar b2;
        ParticleBall p1;

        public Form2()
        {
            InitializeComponent();
            p1 = new ParticleBall
            {
                Mass = 80,
                PositionX = 150,
                PositionY = 500,
                DX = 400,
                DY = 500
            };
            world.Balls.Add(p1);

            b1 = new ParticleBar
            {
                Width = 13,
                Height = 150,
                PositionX = 50,
                PositionY = pictureBox1.Height
            };
            world.Bars.Add(b1);
            b2 = new ParticleBar
            {   
                Width = 13,
                Height = 150,
                PositionX = pictureBox1.Width * 2 + 265,
                PositionY = pictureBox1.Height
            };
            world.Bars.Add(b2);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Width);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            pictureBox1.Image = bmp;
            timer1.Start();

            world.Draw(g);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(Color.White);

            b1.PositionY = p1.PositionY - p1.Mass / 2;
            b2.PositionY = p1.PositionY - p1.Mass / 2;

            world.Draw(g);
            world.DrawRectangle(g);
            pictureBox1.Refresh();

            foreach (var et in world.Balls)
            {
                if (et.PositionX > (pictureBox1.Right - (et.Mass / 2)))
                {
                    //GOL
                    et.PositionX = pictureBox1.Width / 2;
                    et.PositionY = pictureBox1.Height / 2;
                    double theta = 45 + rand.NextDouble() * 90;
                    double randian = Math.PI * theta / 180;
                    et.DX = 600 * Math.Sin(randian);
                    et.DY = 600 * Math.Cos(randian);
                }
                if (et.PositionX < (pictureBox1.Left + (et.Mass / 2)))
                {
                    //GOL
                    et.PositionX = pictureBox1.Width / 2;
                    et.PositionY = pictureBox1.Height / 2;
                    double theta = 45 + rand.NextDouble() * 90;
                    double randian = Math.PI * theta / 180;
                    et.DX = -600 * Math.Sin(randian);
                    et.DY = -600 * Math.Cos(randian);
                }

                if (et.PositionY > (pictureBox1.Top + (et.Mass / 2)))
                {
                    et.DY *= -1;
                }
                if (et.PositionY < (pictureBox1.Bottom - (et.Mass / 2)))
                {
                    et.DY *= -1;
                }

                if (et.PositionX + et.Mass / 2 > b2.PositionX)
                {
                    bool c1 = et.PositionY - et.Mass / 2 < b2.PositionY + b2.Height;
                    bool c2 = et.PositionY + et.Mass / 2 > b2.PositionY;
                    if (c1 && c2)
                        et.DX *= -1.1;
                }

                if (et.PositionX - et.Mass / 2 < b1.PositionX)
                {
                    bool c1 = et.PositionY - et.Mass / 2 < b1.PositionY + b1.Height;
                    bool c2 = et.PositionY + et.Mass / 2 > b1.PositionY;
                    if (c1 && c2)
                        et.DX *= -1.1;
                }
            }
        }
    }
}
