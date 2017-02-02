using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Helpers
{
    public static class LayerHelper
    {
        public static bool SetLayers(List<Node> nodes)
        {
            if (!nodes.Any())
                return true;

            var notProcessedNodes = new List<Node>(nodes);
            var processedNodes = new List<Node>();

            var startNodes = nodes.Where(n => !n.PreviousNodes.Any());
            notProcessedNodes.RemoveAll(n => startNodes.Contains(n));
            processedNodes.AddRange(startNodes);

            var currentLayer = 1;
            while (notProcessedNodes.Any())
            {
                var nextLayerNodes = notProcessedNodes
                    .Where(n => processedNodes.Any(pn => pn.NextNodes.Contains(n)));
                foreach (var node in nextLayerNodes)
                {
                    node.Layer = currentLayer;
                    notProcessedNodes.Remove(node);
                    processedNodes.Add(node);
                }

                currentLayer++;
            }

            return true;
        }
    }
}
