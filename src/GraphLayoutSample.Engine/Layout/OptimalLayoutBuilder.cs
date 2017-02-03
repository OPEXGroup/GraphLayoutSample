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
            return currentSize;
        }
        #endregion

        #region private

        

        #endregion
    }
}
