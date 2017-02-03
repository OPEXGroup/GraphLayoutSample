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

        public RectangleSize SetPositions(IReadOnlyList<Node> nodeGraph, RectangleSize currentSize)
        {
            var random = new Random();
            foreach (var node in nodeGraph)
            {
                node.Position.X = Margin + random.NextDouble() * (Width - node.Width - Margin);
                node.Position.Y = Margin + random.NextDouble() * (Height - node.Height - Margin);
                Logger.LogDebug("RANDOM LAY", $"node {node.Guid}: x = {node.Position.X}, y = {node.Position.Y}");
            }

            return currentSize;
        }

        #endregion

        public RandomLayoutBuilder(double width, double height, double margin)
        {
            Width = width;
            Height = height;
            Margin = margin;
        }

        private double Width { get; }
        private double Height { get; }
        private double Margin { get; }
    }
}
