using System;
using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Enums;
using GraphLayoutSample.Engine.Models;
using GraphLayoutSample.Engine.Utils;

namespace GraphLayoutSample.Engine.Helpers
{
    public static class GraphHelper
    {
        public static List<Node> GenerateRandomGraph(RandomGraphSettings settings)
        {
            var graph = new List<Node>();

            for (var i = 0; i < settings.NodeCount; ++i)
            {
                graph.Add(new Node
                {
                    Height = NextDouble(settings.MinNodeHeight, settings.MaxNodeHeight),
                    Width = NextDouble(settings.MinNodeWidth, settings.MaxNodeWidth),
                });
            }

            while (!TrySetConnections(graph, settings)) { }

            SetPreviousNodes(graph);
            AdjustHeights(graph, settings);
            LayerHelper.SetLayers(graph);

            return graph;
        }

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

        private static bool TrySetConnections(List<Node> nodes, RandomGraphSettings settings)
        {
            ResetConnections(nodes);

            foreach (var node in nodes)
            {
                var nextNodeCount = Random.Next(settings.MinNodeDegree, settings.MaxNodeDegree);

                for (var i = 0; i < nextNodeCount; ++i)
                {
                    node.NextNodes.Add(nodes.GetRandomElement());
                }
            }

            return !HasCycles(nodes);
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

        private static void AdjustHeights(List<Node> nodes, RandomGraphSettings settings)
            => nodes.ForEach(n => n.Height += n.NextNodes.Count * settings.DegreeHeightBonus);

        private static void SetPreviousNodes(IReadOnlyCollection<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.PreviousNodes = nodes.Where(n => n.NextNodes.Contains(node)).ToList();
            }
        }

        private static void ResetConnections(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.NextNodes = new List<Node>();
                node.PreviousNodes = new List<Node>();
            }
        }

        private static T GetRandomElement<T>(this IList<T> list) => list[Random.Next(list.Count - 1)];

        private static double NextDouble(double min, double max) => min + (max - min) * Random.NextDouble();

        private static readonly Random Random = new Random();
    }
}
