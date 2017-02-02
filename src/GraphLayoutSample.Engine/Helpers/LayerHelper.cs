﻿using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Helpers
{
    public static class LayerHelper
    {
        public static void SetLayers(List<Node> nodes)
        {
            if (!nodes.Any())
                return;

            var notProcessedNodes = new List<Node>(nodes);
            var processedNodes = new List<Node>();

            var startNodes = nodes.Where(n => !nodes.Any(nn => nn.NextNodes.Contains(n))).ToList();
            foreach (var node in startNodes)
            {
                node.Layer = 0;
            }
            notProcessedNodes.RemoveAll(n => startNodes.Contains(n));
            processedNodes.AddRange(startNodes);

            var currentLayer = 1;
            while (notProcessedNodes.Any())
            {
                var nextLayerNodes = notProcessedNodes
                    .Where(n => processedNodes.Any(pn => pn.NextNodes.Contains(n)))
                    .ToList();
                foreach (var node in nextLayerNodes)
                {
                    node.Layer = currentLayer;
                    notProcessedNodes.Remove(node);
                    processedNodes.Add(node);
                }

                currentLayer++;
            }
        }
    }
}
