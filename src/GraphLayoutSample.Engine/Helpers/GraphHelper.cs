// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // ToDo check settings
            var graph = new List<Node>();

            for (var i = 0; i < settings.NodeCount; ++i)
            {
                graph.Add(new Node
                {
                    Height = NextDouble(settings.MinNodeHeight, settings.MaxNodeHeight),
                    Width = NextDouble(settings.MinNodeWidth, settings.MaxNodeWidth),
                });
            }

            var startNode = graph.GetRandomElement();
            startNode.Layer = 0;

            var nonStartNodes = graph.Except(new[] {startNode}).ToList();
            foreach (var node in nonStartNodes)
            {
                node.Layer = Random.Next(1, settings.LayerCount);
            }

            BalanceLayers(graph, settings);

            for (var i = 0; i < settings.LayerCount - 1; ++i)
            {
                var layer = graph.Where(n => n.Layer == i).ToList();
                var nextLayer = graph.Where(n => n.Layer == i + 1).ToList();
                var prevLayers = graph.Where(n => n.Layer <= i && n.Layer > 0).ToList();
                foreach (var node in layer)
                {
                    var nextNodeCount = Random.Next(settings.MinNodeDegree, settings.MaxNodeDegree + 1);
                    var nextLayerNodes = nextNodeCount;//prevLayers.Any() ? Random.Next(1, nextNodeCount + 1) : nextNodeCount;

                    for (var j = 0; j < nextLayerNodes; ++j)
                    {
                        node.NextNodes.Add(nextLayer.GetRandomElement());
                    }

                    var prevNodes = nextNodeCount - nextLayerNodes;
                    var prevNodesConnected = 0;
                    var attempts = 0;
                    while (prevNodesConnected < prevNodes && attempts < 10 * prevNodes)
                    {
                        var prevNode = prevLayers.GetRandomElement();
                        if (TryAddBackConnection(node, prevNode, graph))
                            prevNodesConnected++;

                        attempts++;
                    }

                    for (var j = 0; j < prevNodes - prevNodesConnected; ++j)
                    {
                        node.NextNodes.Add(nextLayer.GetRandomElement());
                    }
                }
            }

            foreach (var node in graph)
            {
                Debug.WriteLine($"{node.Guid}: {node.Layer} {node.Degree}");
            }

            SetPreviousNodes(graph);
            AdjustHeights(graph, settings);
            LayerHelper.SetLayers(graph);

            return graph;
        }

        private static bool TryAddBackConnection(Node firstNode, Node secondNode, List<Node> graph)
        {
            firstNode.NextNodes.Add(secondNode);
            if (HasCycles(graph))
            {
                firstNode.NextNodes.Remove(secondNode);
                return false;
            }

            return true;
        }

        public static bool HasCycles(List<Node> nodes)
        {
            if (!nodes.Any())
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

        private static void BalanceLayers(IReadOnlyCollection<Node> graph, RandomGraphSettings settings)
        {
            var emptyLayers = Enumerable
                .Range(1, settings.LayerCount)
                .Where(layer => graph.All(n => n.Layer != layer))
                .ToList();

            foreach (var emptyLayer in emptyLayers)
            {
                var largeLayers = Enumerable
                    .Range(1, settings.LayerCount)
                    .Where(layer => graph.Count(n => n.Layer == layer) > 1)
                    .ToList();
                if (! largeLayers.Any())
                    continue;

                var donorLayer = largeLayers.GetRandomElement();
                graph.First(n => n.Layer == donorLayer).Layer = emptyLayer;
            }

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

        private static T GetRandomElement<T>(this IList<T> list) => list[Random.Next(list.Count)];

        private static double NextDouble(double min, double max) => min + (max - min) * Random.NextDouble();

        private static readonly Random Random = new Random();
    }
}
