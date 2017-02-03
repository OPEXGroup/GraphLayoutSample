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
            var graph = new List<Node>();

            for (var i = 0; i < settings.NodeCount; ++i)
            {
                graph.Add(new Node
                {
                    Height = NextDouble(settings.MinNodeHeight, settings.MaxNodeHeight),
                    Width = NextDouble(settings.MinNodeWidth, settings.MaxNodeWidth),
                });
            }

            SetNodesLayers(graph, settings);
            PrintDebugInfo(graph);

            for (var i = 0; i < settings.LayerCount - 1; ++i)
            {
                var layer = graph.Where(n => n.Layer == i).ToList();
                var nextLayer = graph.Where(n => n.Layer == i + 1).ToList();
                var prevLayers = graph.Where(n => n.Layer <= i && n.Layer > 0).ToList();
                foreach (var node in layer)
                {
                    var nextNodeCount = Math.Max(Random.Next(settings.MinNodeDegree, settings.MaxNodeDegree + 1), nextLayer.Count);
                    var nextLayerNodes = prevLayers.Any() ? Random.Next(1, nextNodeCount + 1) : nextNodeCount;

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

            EnsureGraphIsConnected(graph);
            PrintDebugInfo(graph);

            SetPreviousNodes(graph);
            AdjustHeights(graph, settings);
            LayerHelper.SetLayers(graph);
            PrintDebugInfo(graph);

            return graph;
        }

        private static void EnsureGraphIsConnected(IReadOnlyCollection<Node> graph)
        {
            var layerCount = graph.Select(n => n.Layer).Distinct().Count();
            for (var i = 1; i < layerCount; ++i)
            {
                var prevLayer = graph.Where(n => n.Layer == i - 1).ToList();
                var currentLayer = graph.Where(n => n.Layer == i).ToList();

                var notConnectedNodes = currentLayer.Where(n => !prevLayer.Any(pln => pln.NextNodes.Contains(n)));
                foreach (var notConnectedNode in notConnectedNodes)
                {
                    foreach (var node in prevLayer.Where(p => p.Degree > 1).Shuffle(Random))
                    {
                        var redundantConnection = node.NextNodes
                            .FirstOrDefault(n => n.GetInputDegree(prevLayer) > 1);
                        if (redundantConnection == null)
                            continue;

                        node.NextNodes.Remove(redundantConnection);
                        node.NextNodes.Add(notConnectedNode);
                        break;
                    }
                }
            }
        }

        private static void SetNodesLayers(List<Node> graph, RandomGraphSettings settings)
        {
            var startNode = graph.GetRandomElement();
            startNode.Layer = 0;

            var nonStartNodes = graph.Except(new[] { startNode }).ToList();
            foreach (var node in nonStartNodes)
            {
                node.Layer = Random.Next(1, settings.LayerCount);
            }

            BalanceLayers(graph, settings);
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
                .Range(1, settings.LayerCount - 1)
                .Where(layer => graph.All(n => n.Layer != layer))
                .ToList();

            foreach (var emptyLayer in emptyLayers)
            {
                var largeLayers = Enumerable
                    .Range(1, settings.LayerCount - 1)
                    .Where(layer => graph.Count(n => n.Layer == layer) > 1)
                    .ToList();

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

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            var elements = source.ToArray();
            for (var i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        #region DEBUG

        [Conditional("DEBUG")]
        private static void PrintLayersCount(IReadOnlyCollection<Node> graph)
        {
            foreach (var i in Enumerable.Range(0, graph.Select(n => n.Layer).Distinct().Count()))
            {
                Debug.WriteLine($"Layer {i}: {graph.Count(n => n.Layer == i)}");
            }
        }

        private static int GetInputDegree(this Node node, IEnumerable<Node> graph)
        {
            return graph.SelectMany(n => n.NextNodes.Where(nn => nn == node)).Count();
        }

        private static void PrintNodeDegrees(IReadOnlyCollection<Node> graph)
        {
            var orderedGraph = graph.OrderBy(n => n.Layer);
            foreach (var node in orderedGraph)
            {
                Debug.WriteLine($"Node {node.Guid}: layer {node.Layer}, degree {node.Degree} inputDegree {node.GetInputDegree(graph)}");
            }
        }

        private static void PrintDebugInfo(IReadOnlyCollection<Node> graph)
        {
            PrintLayersCount(graph);
            PrintNodeDegrees(graph);
        }

        #endregion
    }
}
