// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
        public int Degree => NextNodes?.Count ?? -1;

        public Position Position { get; set; } = new Position();

        public int Layer { get; set; }
        public int CoLayer { get; set; }

    }
}
