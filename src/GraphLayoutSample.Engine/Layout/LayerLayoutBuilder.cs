// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Interfaces;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Layout
{
    public class LayerLayoutBuilder : ILayoutBuilder
    {
        public double SetPositions(List<Node> nodeGraph)
        {
            var offset = 25.0;
            var margin = 25.0;

            var layerCount = nodeGraph.Max(n => n.Layer) + 1;
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

                offset += layerNodes.Max(n => n.Width) + margin;
            }

            return offset;
        }
    }
}
