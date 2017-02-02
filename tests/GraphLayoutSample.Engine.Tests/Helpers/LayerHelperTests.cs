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
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void LayerHelper_ThrowsOnNull() => LayerHelper.SetLayers(null);

        [TestMethod]
        public void LayerHelper_DoesNotThrowOnEmptyList() => LayerHelper.SetLayers(new List<Node>());

        [TestMethod]
        public void LayerHelper_SetsLayersForChain()
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
                Assert.AreEqual(i, chain[i].CoLayer);
            }
        }

        [TestMethod]
        public void LayerHelper_SetsLayersForRhombus()
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

            Assert.AreEqual(0, rhombus[0].CoLayer);
            Assert.AreEqual(1, rhombus[1].CoLayer);
            Assert.AreEqual(1, rhombus[1].CoLayer);
            Assert.AreEqual(2, rhombus[3].CoLayer);
        }
    }
}
