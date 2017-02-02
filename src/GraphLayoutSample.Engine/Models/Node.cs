using System;
using System.Collections.Generic;

namespace GraphLayoutSample.Engine.Models
{
    public class Node
    {
        public Guid Guid { get; set; } = new Guid();
        
        public double Width { get; set; }
        public double Height { get; set; }

        public List<Node> NextNodes { get; set; } = new List<Node>();
        public List<Node> PreviousNodes { get; set; } = new List<Node>();

        public Position Position { get; set; } = new Position();

        public int Layer { get; set; }
        public int CoLayer { get; set; }

    }
}
