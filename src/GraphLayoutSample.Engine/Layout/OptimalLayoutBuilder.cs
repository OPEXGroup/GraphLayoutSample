using System;
using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Helpers;
using GraphLayoutSample.Engine.Interfaces;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Layout
{
    public class OptimalLayoutBuilder : ILayoutBuilder
    {
        #region ILayoutBuilder
        public RectangleSize SetPositions(IReadOnlyList<Node> nodeGraph, RectangleSize currentSize)
        {
            const double margin = 25.0;
            var width = SetHorizontalPositions(nodeGraph, margin);
            var layers = SplitGraphByLayer(nodeGraph);
            var layerCount = layers.Count;

            var maxHeight = 0.0;
            for (var i = 1; i < layerCount; ++i)
            {
                var bestPermutation = GetBestPermutation(layers[i - 1], layers[i]);
                var newHeight = SetOrderedLayerVerticalPositions(bestPermutation, margin);
                maxHeight = Math.Max(maxHeight, newHeight);
            }

            return new RectangleSize(width, maxHeight);
        }
        #endregion

        #region private

        private static List<List<Node>> SplitGraphByLayer(IReadOnlyList<Node> nodeGraph)
        {
            var layerCount = nodeGraph.Select(n => n.Layer).Distinct().Count();
            var layers = new List<List<Node>>(layerCount);

            for (var i = 0; i < layerCount; ++i)
            {
                layers.Add(nodeGraph.Where(n => n.Layer == i).ToList());
            }

            return layers;
        }

        private static double SetOrderedLayerVerticalPositions(IReadOnlyList<Node> layer, double margin)
        {
            var offset = margin;

            foreach (var node in layer)
            {
                node.Position.Y = offset;
                offset += node.Height + margin;
            }

            return offset;
        }

        private static double SetHorizontalPositions(IReadOnlyList<Node> nodeGraph, double margin)
        {
            var offset = margin;

            var layerCount = nodeGraph.Max(n => n.Layer) + 1;
            for (var layer = 0; layer < layerCount; ++layer)
            {
                var layerNodes = nodeGraph.Where(n => n.Layer == layer).ToList();

                foreach (var layerNode in layerNodes)
                {
                    layerNode.Position.X = offset;
                }

                offset += layerNodes.Max(n => n.Width) + margin;
            }

            return offset;
        }

        private static List<Tuple<int, int>> GetAllEdges(List<Node> firstLayer, List<Node> secondLayer)
        {
            var firstLayerCount = firstLayer.Count;
            var edges = new List<Tuple<int, int>>();

            for (var i = 0; i < firstLayerCount; ++i)
            {
                foreach (var node in firstLayer[i].NextNodes)
                {
                    var secondIndex = secondLayer.IndexOf(node);
                    edges.Add(new Tuple<int, int>(i, secondIndex));
                }
            }

            return edges;
        }

        private static bool EdgesIntersect(Tuple<int, int> firstEdge, Tuple<int, int> secondEdge)
        {
            return (firstEdge.Item1 < secondEdge.Item1 && firstEdge.Item2 > secondEdge.Item2)
                || (firstEdge.Item1 > secondEdge.Item1 && firstEdge.Item2 < secondEdge.Item2);
        }

        private static int GetCrossCount(List<Node> firstLayer, List<Node> secondLayer)
        {
            var result = 0;

            var edges = GetAllEdges(firstLayer, secondLayer);
            var edgesCount = edges.Count;

            for (var i = 0; i < edgesCount; ++i)
            {
                for (var j = i + 1; j < edgesCount; ++j)
                {
                    var firstEdge = edges[i];
                    var secondEdge = edges[j];

                    if (EdgesIntersect(firstEdge, secondEdge))
                        result++;
                }
            }

            return result;
        }

        private static IReadOnlyList<Node> GetBestPermutation(List<Node> firstLayer, List<Node> secondLayer)
        {
            var minCrossCount = int.MaxValue;
            IReadOnlyList<Node> result = null;

            foreach (var permutation in secondLayer.GetAllPermutations())
            {
                var enumeratedPermutation = permutation.ToList();
                var crossCount = GetCrossCount(firstLayer, enumeratedPermutation);
                if (crossCount < minCrossCount)
                {
                    minCrossCount = crossCount;
                    result = enumeratedPermutation;
                }
            }

            return result;
        }

        #endregion
    }
}
