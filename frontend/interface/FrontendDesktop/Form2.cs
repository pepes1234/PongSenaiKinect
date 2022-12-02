using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;


namespace FrontendDesktop
{
    public partial class Form2 : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
        {
            Interval = 200
        };

        private bool DeciviceExist = false;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource = null;
        bool blurOn = true;
        static int xHand;
        static int yHand;

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
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!itsShowTime)
                return;
            itsShowTime = false;
            Form2 frm = new Form2();

            var oldimg = pictureBox1.Image;

            Bitmap img = (Bitmap)eventArgs.Frame.Clone();

            if (back != null)
                process(img, back);

            pictureBox1.Image = img;

            if (saveImage)
            {
                back = oldimg as Bitmap;
                if (blurOn)
                    Blur(back);
                saveImage = false;
                return;
            }

            oldimg.Dispose();
        }
        unsafe void process(Bitmap im, Bitmap bk)
        {
            if (blurOn)
                Blur(im);

            var imData = im.LockBits(
                new Rectangle(0, 0, im.Width, im.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            var bkData = bk.LockBits(
                new Rectangle(0, 0, bk.Width, bk.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            byte* pim = (byte*)imData.Scan0.ToPointer();
            byte* pbk = (byte*)bkData.Scan0.ToPointer();

            int wid = im.Width;
            Parallel.For(0, im.Height, y =>
            {
                for (int x = 0; x < wid; x++)
                {
                    int index = 3 * x + y * imData.Stride;

                    byte rim = pim[index + 2];
                    byte gim = pim[index + 1];
                    byte bim = pim[index];

                    byte rbk = pbk[index + 2];
                    byte gbk = pbk[index + 1];
                    byte bbk = pbk[index];

                    int dr = rim - rbk;
                    int dg = gim - gbk;
                    int db = bim - bbk;

                    int diff = dr * dr + dg * dg + db * db;
                    if (x <= N || y <= N || x >= wid - N)
                    {
                        pim[index + 2] = 255;
                        pim[index + 1] = 255;
                        pim[index + 0] = 255;
                    }
                    else if (diff > 1200)
                    {
                        pim[index + 2] = 0;
                        pim[index + 1] = 0;
                        pim[index + 0] = 0;
                    }
                    else
                    {
                        pim[index + 2] = 255;
                        pim[index + 1] = 255;
                        pim[index + 0] = 255;
                    }
                }
            });

            bk.UnlockBits(bkData);
            im.UnlockBits(imData);

            FindHand(im);
        }
        long[,] Rarr = null;
        long[,] Garr = null;
        long[,] Barr = null;
        int N = 30;
        

        public bool DeviceExist { get; private set; }

        int f = 0;
        int[] filterX = new int[5];
        int[] filterY = new int[5];
        int totalX = 0;
        int totalY = 0;
        public unsafe void Blur(Bitmap image)
        {
            if (Rarr == null)
            {
                Rarr = new long[image.Width, image.Height];
                Garr = new long[image.Width, image.Height];
                Barr = new long[image.Width, image.Height];
            }

            int width = image.Width;
            int height = image.Height;
            
            int size = 2 * N + 1;
            int area = size * size;

            var data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            byte* p = (byte*)data.Scan0.ToPointer();

            long[][,] colors = new long[3][,];
            colors[0] = Barr;
            colors[1] = Garr;
            colors[2] = Rarr;

            Parallel.For(0, data.Height, j =>
            {
                byte* l = p + j * data.Stride;
                for (int i = 0; i < data.Width; i++, l += 3)
                {
                    colors[0][i, j] = l[0];
                    colors[1][i, j] = l[1];
                    colors[2][i, j] = l[2];
                }
            });

            Parallel.For(0, 3, c =>
            {
                for (int i = 1; i < width; i++)
                {
                    colors[c][i, 0] += colors[c][i - 1, 0];
                }

                for (int j = 1; j < height; j++)
                {
                    colors[c][0, j] += colors[c][0, j - 1];
                }

                for (int j = 1; j < height; j++)
                {
                    for (int i = 1; i < width; i++)
                    {
                        colors[c][i, j] += colors[c][i - 1, j] + colors[c][i, j - 1] - colors[c][i - 1, j - 1];
                    }
                }
            });

            Parallel.For(0, 3, c =>
            {
                Parallel.For(N + 1, image.Height - N, j =>
                {
                    byte* l = p + j * data.Stride + 3 * (N + 1);
                    for (int i = (N + 1); i < data.Width - N; i++, l += 3)
                    {
                        long Y = colors[c][i - N - 1, j - N - 1];
                        long X = colors[c][i - N - 1, j + N];
                        long Z = colors[c][i + N, j - N - 1];
                        long W = colors[c][i + N, j + N];

                        long C = Y + W - Z - X;
                        l[c] = (byte)(C / area);
                    }
                });
            });

            image.UnlockBits(data);
        }
        
        bool saveImage = false;
        Bitmap back = null;
        private unsafe void FindHand(Bitmap img)
        {
            var now = DateTime.Now;

            int cx = 0, cy = 0;
            long sx = 0, sy = 0, count = 0;
            // Retrieve the image.

            var data = img.LockBits(
                new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            byte* p = (byte*)data.Scan0.ToPointer();

            bool flag = false;
            int maxx = int.MaxValue; //int maxx = 0;
            // Loop through the images pixels to reset color.
            for (int y = 0; y < 4 * img.Height / 5; y += 10)
            {
                for (int x = 0; x < img.Width; x += 10) //for (int x = image1.Width - 1; x > 0; x--)
                {
                    int index = 3 * x + y * data.Stride;
                    byte r = p[index + 2];
                    byte g = p[index + 1];
                    byte b = p[index];
                    if (r < 10 && g < 10 && b < 10)
                    {
                        if (x < maxx) //if (x > maxx)
                        {
                            maxx = x;
                            cx = x;
                            cy = y;
                            break;
                        }
                    }
                }
            }

            flag = true;

            int epochs = 20;
            while (flag)
            {
                flag = false;

                epochs--;
                if (epochs == 0)
                    break;

                for (int x = cx - 20; x < cx + 20; x += 10)
                {
                    if (x < 0)
                        continue;
                    int y = cy - 20;

                    int index = 3 * x + y * data.Stride;
                    byte r = p[index + 2];
                    byte g = p[index + 1];
                    byte b = p[index];
                    if (r < 128 && g < 128 && b < 128)
                    {
                        cy -= 20;
                        flag = true;
                        break;
                    }
                }

                sx = 0;
                sy = 0;
                count = 0;
                for (int i = cx - 20; i < cx + 20; i += 10)
                {
                    for (int j = cy - 20; j < cy + 20; j += 10)
                    {
                        if (i < 0 || j < 0)
                            continue;

                        int index = 3 * i + j * data.Stride;
                        byte r = p[index + 2];
                        byte g = p[index + 1];
                        byte b = p[index];
                        if (r < 128 && g < 128 && b < 128)
                        {
                            sx += i;
                            sy += j;
                            count++;
                        }
                    }
                }

                if (count == 0)
                {
                    img.UnlockBits(data);
                    return;
                }

                cx = (int)(sx / count);
                cy = (int)(sy / count);
            }

            totalX -= filterX[f];
            totalY -= filterY[f];

            filterX[f] = cx;
            filterY[f] = cy;
            totalX += cx;
            totalY += cy;
            f++;
            if (f == filterX.Length)
                f = 0;

            cx = totalX / filterX.Length;
            cy = totalY / filterX.Length;

            sx = 0;
            sy = 0;
            count = 0;
            for (int i = cx - 80; i < cx + 80; i += 10)
            {
                for (int j = cy - 80; j < cy + 80; j += 10)
                {
                    if (i < 0 || j < 0)
                        continue;

                    int index = 3 * i + j * data.Stride;
                    byte r = p[index + 2];
                    byte g = p[index + 1];
                    byte b = p[index];
                    if (r < 128 && g < 128 && b < 128)
                    {
                        sx += i;
                        sy += j;
                        count++;
                    }
                }
            }
            cx = (int)(sx / count);
            cy = (int)(sy / count);
            xHand = cx;
            yHand = cy;

            img.UnlockBits(data);

            var _g = Graphics.FromImage(img);
            _g.FillEllipse(Brushes.Red, cx - 5, cy - 5, 10, 10);
            _g.Dispose();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                saveImage = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);

            //Encerra o sinal da camera.
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
            videoSource.DesiredFrameSize = new Size(160, 120);
            videoSource.DesiredFrameRate = 10;
            videoSource.Start();

            timer.Start();


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
        bool itsShowTime = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            itsShowTime = true;
            g.Clear(Color.White);

            b1.PositionY = yHand;
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
