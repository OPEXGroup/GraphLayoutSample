// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Engine.Interfaces
{
    public interface ILayoutBuilder
    {
        /// <summary>
        ///     Layout builder interface
        /// </summary>
        /// <param name="nodeGraph">Node graph to fit</param>
        /// <param name="currectSize">Current field size</param>
        /// <returns>Preferred field size</returns>
        RectangleSize SetPositions(IReadOnlyList<Node> nodeGraph, RectangleSize currectSize);
    }
}
