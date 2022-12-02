using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendDesktop
{
    internal abstract class Bar
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public abstract void DrawRectangle(Graphics g);
    }
}
