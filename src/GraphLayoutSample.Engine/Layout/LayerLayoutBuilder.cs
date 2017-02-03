// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Interfaces;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Layout
{
    public class LayerLayoutBuilder : ILayoutBuilder
    {
        public RectangleSize SetPositions(IReadOnlyList<Node> nodeGraph, RectangleSize currentSize)
        {
            var offset = 25.0;
            var margin = 25.0;

            var layerCount = nodeGraph.Max(n => n.Layer) + 1;
            var height = 0.0;
            for (var layer = 0; layer < layerCount; ++layer)
            {
                var layerNodes = nodeGraph.Where(n => n.Layer == layer).ToList();

                var verticalOffset = margin;
                foreach (var layerNode in layerNodes)
                {
                    layerNode.Position.X = offset;
                    layerNode.Position.Y = verticalOffset;
                    verticalOffset += layerNode.Height + margin;
                }
                height = Math.Max(height, verticalOffset);

                offset += layerNodes.Max(n => n.Width) + margin;
            }

            return new RectangleSize(offset, height);
        }
    }
}
