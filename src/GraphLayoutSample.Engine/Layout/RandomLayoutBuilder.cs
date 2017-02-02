using System;
using System.Collections.Generic;
using GraphLayoutSample.Engine.Interfaces;
using GraphLayoutSample.Engine.Models;
using ITCC.Logging.Core;

namespace GraphLayoutSample.Engine.Layout
{
    public class RandomLayoutBuilder : ILayoutBuilder
    {
        #region ILayoutBuilder

        public double SetPositions(List<Node> nodeGraph)
        {
            var random = new Random();
            foreach (var node in nodeGraph)
            {
                node.Position.X = Margin + random.NextDouble() * (Width - node.Width - Margin);
                node.Position.Y = Margin + random.NextDouble() * (Height - node.Height - Margin);
                Logger.LogDebug("RANDOM LAY", $"node {node.Guid}: x = {node.Position.X}, y = {node.Position.Y}");
            }

            return Width;
        }

        #endregion

        public RandomLayoutBuilder(double width, double height, double margin)
        {
            Width = width;
            Height = height;
            Margin = margin;
        }

        public double Width { get; }
        public double Height { get; }
        public double Margin { get; }
    }
}
