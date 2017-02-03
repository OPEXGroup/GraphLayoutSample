using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLayoutSample.Engine.Models
{
    public class RectangleSize
    {
        public RectangleSize(double width, double heigth)
        {
            Width = width;
            Heigth = heigth;
        }

        public double Width { get; }
        public double Heigth { get; }
    }
}
