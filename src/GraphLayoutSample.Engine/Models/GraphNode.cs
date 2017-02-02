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
