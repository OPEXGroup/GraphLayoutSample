using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Enums;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Helpers
{
    public static class GraphHelper
    {
        public static bool HasCycles(List<Node> nodes)
        {
            if (! nodes.Any())
                return false;

            var graph = BuildGraph(nodes);
            foreach (var graphNode in graph)
            {
                ResetColors(graph);

                if (NodeIsCycleStart(graphNode))
                    return true;
            }

            return false;
        }

        private static bool NodeIsCycleStart(GraphNode node)
        {
            node.Color = NodeColor.Gray;

            foreach (var nextNode in node.NextNodes)
            {
                if (nextNode.Color == NodeColor.Gray)
                    return true;

                if (NodeIsCycleStart(nextNode))
                    return true;
            }

            node.Color = NodeColor.Black;
            return false;
        }

        private static List<GraphNode> BuildGraph(IEnumerable<Node> nodes)
        {
            var graph = nodes.Select(n => new GraphNode
            {
                Node = n,
                Color = NodeColor.White
            }).ToList();

            foreach (var graphNode in graph)
            {
                graphNode.NextNodes = graph.Where(gn => graphNode.Node.NextNodes.Contains(gn.Node)).ToList();
            }

            return graph;
        }

        private static void ResetColors(IEnumerable<GraphNode> nodes)
        {
            foreach (var graphNode in nodes)
            {
                graphNode.Color = NodeColor.White;
            }
        }
    }
}
