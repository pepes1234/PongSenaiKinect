using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendDesktop
{
    internal class ParticleBar : Bar
    {
        public override void DrawRectangle(Graphics g)
        {
            Pen p1 = new Pen(Color.Black, 8);
            g.DrawRectangle(p1, (float)PositionX, (float)PositionY, (float)Width, (float)Height);
            g.FillRectangle(Brushes.LightGray, (float)PositionX, (float)PositionY, (float)Width, (float)Height);
        }
    }
}
