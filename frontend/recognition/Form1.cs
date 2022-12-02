namespace SenaiRecognition;
using System.IO;
using System;

public partial class Form1 : Form
{
    Label lb1;
    PictureBox pb1;
    PictureBox pb2;
    List<Panel> pnlist = new List<Panel>();
    List<Panel> pnlist2 = new List<Panel>();
    Bitmap bmp;
    Bitmap bmp1;
    public Form1()
    {
        InitializeComponent();

        Load += delegate
        {
            lb1 = new Label();
            lb1.AutoSize = true;
            this.Controls.Add(lb1);
            
            pb1 = new PictureBox();
            pb1.Image = Image.FromFile(@"TestImages\imagem8.jpeg");
            pb1.Size = new Size(2 * 246, 2 * 164);
            pb1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pb1);
            pb1.Location = new Point(10, 10);
            bmp = new Bitmap(@"TestImages\imagem8.jpeg", true);

            pb2 = new PictureBox();
            pb2.Image = Image.FromFile(@"TestImages\imagem4.jpeg");
            pb2.Size = new Size(2 * 246, 2 * 164);
            pb2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pb2);
            pb2.Location = new Point(10, 350);
            bmp1 = new Bitmap(@"TestImages\imagem4.jpeg", true);

            for(int i = 0; i < 10; i++)
            {
                pnlist.Add(new Panel());
                pnlist[i].Size = new Size(25, 25);
                pnlist[i].Location = new Point(this.Width - 300, i * 40 + pnlist[i].Height);
                pnlist[i].BackColor = Color.Gray;
                this.Controls.Add(pnlist[i]);
            }

            
            for(int i = 0; i < 10; i++)
            {
                pnlist2.Add(new Panel());
                pnlist2[i].Size = new Size(25, 25);
                pnlist2[i].Location = new Point(this.Width - 200, i * 40 + pnlist2[i].Height);
                pnlist2[i].BackColor = Color.Gray;
                this.Controls.Add(pnlist2[i]);
            }

            KMeans kms = new KMeans();

            var now = DateTime.Now;
            Color[] clr = kms.Fit(bmp, 10);
            MessageBox.Show((DateTime.Now - now).Milliseconds.ToString());

            for(int i = 0; i < 10; i++)
            {
                pnlist[i].BackColor = clr[i];
            }

            Color[] clr1 = kms.Fit(bmp1, 10);
            
            for(int i = 0; i < 10; i++)
            {
                pnlist2[i].BackColor = clr1[i];
            }
            
            double[] diffForColor = new double[10];
            double mindiffPerColor = 0;
            double mindif = double.PositiveInfinity;
            double diff = 0;
            
            for(int i = 0; i <= 10; i++)
            {
                int clrR = clr[i].R;
                int clrG = clr[i].G;
                int clrB = clr[i].B;

                for (int j = 0; j <= 10; j++)
                {       
                    int clr1R = clr1[j].R;
                    int clr1G = clr1[j].G;
                    int clr1B = clr1[j].B;

                    long dr = clrR - clr1R;
                    long dg = clrG - clr1G;
                    long db = clrB - clr1B;

                    diff = dr * dr + dg * dg + db * db;
                    if(mindif > diff)
                    {
                        mindif = diff;
                    }
                }            
                diffForColor[i] = mindif;
            }
            string lbText = "";
            for(int i = 0; i < 11; i++)
            {
                lbText += "Color " + i + " diff=" +  diffForColor[i].ToString() + " ";
            }
            lb1.Text = lbText;
        };
    }
}
