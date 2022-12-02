using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace Blur;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Load += delegate
        {
            ColocarImagemNaTela();
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public unsafe void ColocarImagemNaTela() 
    {
        Bitmap bitmapImage = new Bitmap(@"superman.jpg", true);
        int width = bitmapImage.Width;
        int height = bitmapImage.Height;

        int N = 20;
        int size = 2 * N + 1;
        int area = size * size;
        // 5 min 10 seg
        this.pictureBox1.Image = bitmapImage;

        var now = DateTime.Now;

        long[,] Rarr  = new long[bitmapImage.Width, bitmapImage.Height];
        long[,] Garr  = new long[bitmapImage.Width, bitmapImage.Height];
        long[,] Barr  = new long[bitmapImage.Width, bitmapImage.Height];

        var data = bitmapImage.LockBits(
            new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height),
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
            for(int i = 0; i < data.Width; i++, l += 3)
            {
                colors[0][i, j] = l[0];
                colors[1][i, j] = l[1];
                colors[2][i, j] = l[2];
            }
        });

        Parallel.For(0, 3, c =>
        {
            for(int i = 1; i < width; i++)
            {
                colors[c][i, 0] += colors[c][i - 1, 0];          
            }

            for(int j = 1; j < height; j++)
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
            Parallel.For(N + 1, bitmapImage.Height - N, j=>
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

        bitmapImage.UnlockBits(data);
        MessageBox.Show((DateTime.Now - now).TotalMilliseconds.ToString());
    }
}