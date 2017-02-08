// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Helpers
{
    public static class LayerHelper
    {
        public static void SetLayers(List<Node> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

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

        public static void SetCoLayers(List<Node> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            if (!nodes.Any())
                return;

            if (GraphHelper.HasCycles(nodes))
                throw new InvalidOperationException("Colayers are defined only for acyclic graphs");

            var startNodes = nodes.Where(n => !nodes.Any(nn => nn.NextNodes.Contains(n))).ToList();
            foreach (var node in startNodes)
            {
                node.CoLayer = 0;
                SetSubtreeCoLayers(node);
            }
        }

        private static void SetSubtreeCoLayers(Node startNode)
        {
            var baseCoLayer = startNode.CoLayer;
            foreach (var nextNode in startNode.NextNodes)
            {
                nextNode.CoLayer = Math.Max(nextNode.CoLayer, baseCoLayer + 1);
                SetSubtreeCoLayers(nextNode);
            }
        }

        public static int GetLayerCount(this IEnumerable<Node> graph) => graph.Select(n => n.Layer).Distinct().Count();
    }
}
