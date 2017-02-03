using System.Collections.Generic;
using GraphLayoutSample.Engine.Interfaces;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Layout
{
    public class OptimalLayoutBuilder : ILayoutBuilder
    {
        #region ILayoutBuilder
        public double SetPositions(IReadOnlyList<Node> nodeGraph, double currentWidth, double currentHeight)
        {
            return currentWidth;
        }
        #endregion
    }
}
