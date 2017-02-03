using System;
using System.Collections.Generic;
using System.Linq;
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

            var height = SetAllOrderedLayersVerticalPositions(layers, margin);

            return new RectangleSize(width, height);
        }
        #endregion

        #region private

        private static List<IReadOnlyList<Node>> SplitGraphByLayer(IReadOnlyList<Node> nodeGraph)
        {
            var layerCount = nodeGraph.Select(n => n.Layer).Distinct().Count();
            var layers = new List<IReadOnlyList<Node>>(layerCount);

            for (var i = 0; i < layerCount; ++i)
            {
                layers.Add(nodeGraph.Where(n => n.Layer == i).ToList());
            }

            return layers;
        }

        private static double SetAllOrderedLayersVerticalPositions(List<IReadOnlyList<Node>> layers, double margin)
        {
            var maxHeight = 0.0;

            foreach (var layer in layers)
            {
                maxHeight = Math.Max(maxHeight, SetOrderedLayerVerticalPositions(layer, margin));
            }

            return maxHeight;
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

        private static int GetCrossCount(IReadOnlyList<Node> firstLayer, IReadOnlyList<Node> secondLayer)
        {
            return 0;
        }

        #endregion
    }
}
