using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

//bgr
public class KMeans
{
    public unsafe Color[] Fit(Bitmap bmp, int K)
    {
        Color[] centroids = new Color[K];
        Random rando = new Random();
        for (int i = 0; i < K; i++)
        {
            var r = rando.Next(255);
            var g = rando.Next(255);
            var b = rando.Next(255);
            centroids[i] = Color.FromArgb(r, g, b);
        }
        long avarageR = 0;
        long avarageG = 0;
        long avarageB = 0;
            
        var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
            ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        byte* pointer = (byte*)data.Scan0.ToPointer();

        int epochs = 0;
        while (epochs < 40)
        {
            epochs++;
            List<(long, long, long, long)> sums = new List<(long, long, long, long)>();
            for (int i = 0; i < K; i++)
                sums.Add((0, 0, 0, 0));

            int start = rando.Next(2);

            Parallel.For(start, data.Height / 3, y =>
            {
                int jump = rando.Next(2, 12);
                y = 3 * y;
                byte* line = pointer + y * data.Stride;
                for(int x = start; x < 3 * data.Width; x += 3 + jump, line += 3)
                {
                    int bestcentroidindex = -1;
                    double minDist = double.PositiveInfinity;
                    
                    for(int i = 0; i < K; i++)
                    {
                        int centroidR = centroids[i].R;
                        int centroidG = centroids[i].G;
                        int centroidB = centroids[i].B;
                        
                        int clrPixelBmpR = line[2];
                        int clrPixelBmpG = line[1];
                        int clrPixelBmpB = line[0];

                        int dr = centroidR - clrPixelBmpR;
                        int dg = centroidG - clrPixelBmpG;
                        int db = centroidB - clrPixelBmpB;

                        double pitagoras = dr * dr + dg * dg + db * db;
                        
                        if(minDist > pitagoras)
                        {
                            bestcentroidindex = i;
                            minDist = pitagoras;
                        }
                    }
                    
                    var sum = sums[bestcentroidindex];
                    sums[bestcentroidindex] = (sum.Item1 + line[2], sum.Item2 + line[1], sum.Item3 + line[0], sum.Item4 + 1);
                }
            });

            double mindif = double.PositiveInfinity;
            double diff = 0;
            for(int i = 0; i < K; i++)
            {
                if (sums[i].Item4 == 0)
                    continue;
                avarageR = sums[i].Item1 / sums[i].Item4;
                avarageG = sums[i].Item2 / sums[i].Item4;
                avarageB = sums[i].Item3 / sums[i].Item4;
                centroids[i] = Color.FromArgb((int)avarageR, (int)avarageG, (int)avarageB);

                long centroidR = centroids[i].R;
                long centroidG = centroids[i].G;
                long centroidB = centroids[i].B;

                long dr = centroidR - avarageR;
                long dg = centroidG - avarageG;
                long db = centroidB - avarageB;

                diff = dr * dr + dg * dg + db * db;

                if(diff < mindif)
                {
                    mindif = diff;
                }
            }
            if(mindif > 10)
            {
                return centroids;
            }
        }
        bmp.UnlockBits(data);

        return centroids;
    }
}