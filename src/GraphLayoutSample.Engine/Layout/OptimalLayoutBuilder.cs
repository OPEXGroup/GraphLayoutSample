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
            var layerCount = nodeGraph.Select(n => n.Layer).Distinct().Count();

            return currentSize;
        }
        #endregion

        #region private

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
