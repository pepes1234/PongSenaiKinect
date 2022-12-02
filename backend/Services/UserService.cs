namespace backend.Services;
using System.Drawing;

using Model;
public class UserService
{
    public bool VerifyKmeans(Color[] clr1, Color[]clr2)
    {
        double[] diffForColor = new double[clr1.Length];
        double mindif = double.PositiveInfinity;
        double diff = 0;
        double result = 0;
        
        for(int i = 0; i <= clr1.Length - 1; i++)
        {
            int clrR1 = clr1[i].R;
            int clrG1 = clr1[i].G;
            int clrB1 = clr1[i].B;

            for (int j = 0; j <= clr2.Length - 1; j++)
            {       
                int clr2R = clr2[j].R;
                int clr2G = clr2[j].G;
                int clr2B = clr2[j].B;

                long dr = clrR1 - clr2R;
                long dg = clrG1 - clr2G;
                long db = clrB1 - clr2B;

                diff = dr * dr + dg * dg + db * db;
                if(mindif > diff)
                {
                    mindif = diff;
                }
            }
            diffForColor[i] = mindif;
        }
        for(int i = 0; i < diffForColor.Length; i++)
        {
            result += diffForColor[i];
        }
        if(result >= 400)
            return false;
        return true;
    }
}