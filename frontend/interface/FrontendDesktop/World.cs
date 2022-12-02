using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendDesktop
{
    internal class World
    {
        public List<Ball> Balls = new List<Ball>();
        public List<Bar> Bars = new List<Bar>();

        public void Draw(Graphics g)
        {
            foreach (var e in Balls)
            {
                e.PositionX += e.DX / 50;
                e.PositionY += e.DY / 50;
                e.DY += 9.8 / 50;
                e.Draw(g);
            }
        }
        public void DrawRectangle(Graphics g)
        {
            foreach(var e in Bars)
            {
                e.DrawRectangle(g);
            }
        }
    }
}
