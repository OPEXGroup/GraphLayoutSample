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
        /// <returns>Preferred field width</returns>
        double SetPositions(List<Node> nodeGraph);
    }
}
