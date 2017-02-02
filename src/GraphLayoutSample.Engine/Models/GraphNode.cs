// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using GraphLayoutSample.Engine.Enums;

namespace GraphLayoutSample.Engine.Models
{
    public class GraphNode
    {
        public Node Node { get; set; }
        public NodeColor Color { get; set; }

        public List<GraphNode> NextNodes { get; set; } = new List<GraphNode>();
    }
}
