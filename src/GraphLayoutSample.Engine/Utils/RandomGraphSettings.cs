﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace GraphLayoutSample.Engine.Utils
{
    public class RandomGraphSettings
    {
        #region properties

        public int NodeCount { get; set; } = DefaultNodeCount;
        public int LayerCount { get; set; } = DefaultLayerCount;

        public int MinNodeDegree { get; set; } = DefaultMinNodeDegree;
        public int MaxNodeDegree { get; set; } = DefaultMaxNodeDegree;

        public double MinNodeWidth { get; set; } = DefaultMinNodeWidth;
        public double MaxNodeWidth { get; set; } = DefaultMaxNodeWidth;

        public double MinNodeHeight { get; set; } = DefaultMinNodeHeight;
        public double MaxNodeHeight { get; set; } = DefaultMaxNodeHeight;

        /// <summary>
        ///     Real height = generated height + (degree * DegreeHeightBonus)
        /// </summary>
        public double DegreeHeightBonus { get; set; } = DefaultDegreeHeightBonus;
        #endregion

        #region constants

        private const int DefaultNodeCount = 15;
        private const int DefaultLayerCount = 7;

        private const int DefaultMinNodeDegree = 1;
        private const int DefaultMaxNodeDegree = 5;

        private const double DefaultMinNodeWidth = 150;
        private const double DefaultMaxNodeWidth = 220;

        private const double DefaultMinNodeHeight = 50;
        private const double DefaultMaxNodeHeight = 100;

        private const double DefaultDegreeHeightBonus = 15;

        #endregion
    }
}
