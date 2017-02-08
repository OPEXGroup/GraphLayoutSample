// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using GraphLayoutSample.Engine.Helpers;
using GraphLayoutSample.Engine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphLayoutSample.Engine.Tests.Helpers
{
    [TestClass]
    public class LayerHelperTests
    {
        #region SetLayers
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetLayers_ThrowsOnNull() => LayerHelper.SetLayers(null);

        [TestMethod]
        public void SetLayers_DoesNotThrowOnEmptyList() => LayerHelper.SetLayers(new List<Node>());

        [TestMethod]
        public void SetLayers_SetsLayersForChain()
        {
            var chain = new List<Node>
            {
                new Node(),
                new Node(),
                new Node()
            };

            chain[0].NextNodes = new List<Node> { chain[1] };
            chain[1].NextNodes = new List<Node> { chain[2] };

            LayerHelper.SetLayers(chain);

            for (var i = 0; i < chain.Count; ++i)
            {
                Assert.AreEqual(i, chain[i].Layer);
            }
        }

        [TestMethod]
        public void SetLayers_SetsLayersForRhombus()
        {
            var rhombus = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            rhombus[0].NextNodes = new List<Node> { rhombus[1], rhombus[2] };
            rhombus[1].NextNodes = new List<Node> { rhombus[3] };
            rhombus[2].NextNodes = new List<Node> { rhombus[3] };

            LayerHelper.SetLayers(rhombus);

            Assert.AreEqual(0, rhombus[0].Layer);
            Assert.AreEqual(1, rhombus[1].Layer);
            Assert.AreEqual(1, rhombus[1].Layer);
            Assert.AreEqual(2, rhombus[3].Layer);
        }

        [TestMethod]
        public void SetLayers_SetsLayersForComplexGraph()
        {
            var rhombus = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            rhombus[0].NextNodes = new List<Node> { rhombus[1], rhombus[2] };
            rhombus[1].NextNodes = new List<Node> { rhombus[3] };
            rhombus[2].NextNodes = new List<Node> { rhombus[4] };
            rhombus[3].NextNodes = new List<Node> { rhombus[4] };

            LayerHelper.SetLayers(rhombus);

            Assert.AreEqual(0, rhombus[0].Layer);
            Assert.AreEqual(1, rhombus[1].Layer);
            Assert.AreEqual(1, rhombus[1].Layer);
            Assert.AreEqual(2, rhombus[3].Layer);
            Assert.AreEqual(2, rhombus[4].Layer);
        }
        
        #endregion

        #region SetCoLayers

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCoLayers_ThrowsOnNull() => LayerHelper.SetCoLayers(null);

        [TestMethod]
        public void SetCoLayers_DoesNotThrowOnEmptyList() => LayerHelper.SetCoLayers(new List<Node>());

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void SetCoLayers_ThrowsOnGraphWithCycles()
        {
            var cycle = new List<Node>
            {
                new Node(),
                new Node(),
                new Node()
            };

            cycle[0].NextNodes = new List<Node> { cycle[1] };
            cycle[1].NextNodes = new List<Node> { cycle[2] };
            cycle[2].NextNodes = new List<Node> { cycle[0] };

            LayerHelper.SetCoLayers(cycle);
        }

        [TestMethod]
        public void SetCoLayers_SetsCoLayersForChain()
        {
            var chain = new List<Node>
            {
                new Node(),
                new Node(),
                new Node()
            };

            chain[0].NextNodes = new List<Node> { chain[1] };
            chain[1].NextNodes = new List<Node> { chain[2] };

            LayerHelper.SetCoLayers(chain);

            for (var i = 0; i < chain.Count; ++i)
            {
                Assert.AreEqual(i, chain[i].CoLayer);
            }
        }

        [TestMethod]
        public void SetCoLayers_SetsCoLayersForRhombus()
        {
            var rhombus = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            rhombus[0].NextNodes = new List<Node> { rhombus[1], rhombus[2] };
            rhombus[1].NextNodes = new List<Node> { rhombus[3] };
            rhombus[2].NextNodes = new List<Node> { rhombus[3] };

            LayerHelper.SetCoLayers(rhombus);

            Assert.AreEqual(0, rhombus[0].CoLayer);
            Assert.AreEqual(1, rhombus[1].CoLayer);
            Assert.AreEqual(1, rhombus[1].CoLayer);
            Assert.AreEqual(2, rhombus[3].CoLayer);
        }

        [TestMethod]
        public void SetCoLayers_SetsCoLayersForComplexGraph()
        {
            var rhombus = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            rhombus[0].NextNodes = new List<Node> { rhombus[1], rhombus[2] };
            rhombus[1].NextNodes = new List<Node> { rhombus[3] };
            rhombus[2].NextNodes = new List<Node> { rhombus[4] };
            rhombus[3].NextNodes = new List<Node> { rhombus[4] };

            LayerHelper.SetCoLayers(rhombus);

            Assert.AreEqual(0, rhombus[0].CoLayer);
            Assert.AreEqual(1, rhombus[1].CoLayer);
            Assert.AreEqual(1, rhombus[1].CoLayer);
            Assert.AreEqual(2, rhombus[3].CoLayer);
            Assert.AreEqual(3, rhombus[4].CoLayer);
        }

        #endregion

        #region GetLayerCount

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public static void GetLayerCount_ThrowsOnNullGraph() => LayerHelper.GetLayerCount(null);

        [TestMethod]
        public static void GetLayerCount_ReturnsCorrectLayerCount()
        {
            var rhombus = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            rhombus[0].NextNodes = new List<Node> { rhombus[1], rhombus[2] };
            rhombus[1].NextNodes = new List<Node> { rhombus[3] };
            rhombus[2].NextNodes = new List<Node> { rhombus[4] };
            rhombus[3].NextNodes = new List<Node> { rhombus[4] };

            LayerHelper.SetLayers(rhombus);

            Assert.AreEqual(4, rhombus.GetLayerCount());
        }

        #endregion
    }
}
