// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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

        public double SetPositions(IReadOnlyList<Node> nodeGraph, double currentWidth, double currentHeight)
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
