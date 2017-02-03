// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using GraphLayoutSample.Engine.Helpers;
using GraphLayoutSample.Engine.Models;
using GraphLayoutSample.Engine.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphLayoutSample.Engine.Tests.Helpers
{
    [TestClass]
    public class GraphHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void HasCycles_ThrowsOnNullList() => GraphHelper.HasCycles(null);

        [TestMethod]
        public void HasCycles_ReturnsFalseOnEmptyGraph()
        {
            var emptyGraph = new List<Node>();

            Assert.AreEqual(false, GraphHelper.HasCycles(emptyGraph));
        }

        [TestMethod]
        public void HasCycles_ReturnsFalseOnChain()
        {
            var chain = new List<Node>
            {
                new Node(),
                new Node(),
                new Node()
            };

            chain[0].NextNodes = new List<Node> { chain[1] };
            chain[1].NextNodes = new List<Node> { chain[2] };

            Assert.AreEqual(false, GraphHelper.HasCycles(chain));
        }

        [TestMethod]
        public void HasCycles_ReturnsFalseOnTree()
        {
            var tree = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            tree[0].NextNodes = new List<Node> { tree[1], tree[2] };
            tree[1].NextNodes = new List<Node> { tree[3], tree[5] };
            tree[2].NextNodes = new List<Node> { tree[4], tree[6] };

            Assert.AreEqual(false, GraphHelper.HasCycles(tree));
        }

        [TestMethod]
        public void HasCycles_ReturnsTrueOnCycle()
        {
            var cycle = new List<Node>
            {
                new Node(),
                new Node(),
                new Node(),
                new Node()
            };

            cycle[0].NextNodes = new List<Node> { cycle[1] };
            cycle[1].NextNodes = new List<Node> { cycle[2] };
            cycle[2].NextNodes = new List<Node> { cycle[3] };
            cycle[3].NextNodes = new List<Node> { cycle[0] };

            Assert.AreEqual(true, GraphHelper.HasCycles(cycle));
        }

        [TestMethod]
        public void GenerateRandomGraph_GeneratesAcyclcGraph()
        {
            var settings = new RandomGraphSettings();
            var graph = GraphHelper.GenerateRandomGraph(settings);

            Assert.AreEqual(false, GraphHelper.HasCycles(graph));
        }

        [TestMethod]
        public void GenerateRandomGraph_GeneratesChainIfNodeCountEqualsLayerCount()
        {
            var settings = new RandomGraphSettings
            {
                NodeCount = 10,
                LayerCount = 10
            };

            var graph = GraphHelper.GenerateRandomGraph(settings);
            var orderedGraph = graph.OrderBy(n => n.Layer).ToList();

            for (var i = 0; i < settings.NodeCount; ++i)
            {
                Assert.AreEqual(i, orderedGraph[i].Layer);
            }
        }

        [TestMethod]
        public void GenerateRandomGraph_GeneratesGraphsWithCorrectParams()
        {
            var settings = new RandomGraphSettings
            {
                NodeCount = 10,
                LayerCount = 4,
                MaxNodeDegree = 5,
                MinNodeDegree = 2
            };

            var graph = GraphHelper.GenerateRandomGraph(settings);

            var distinctLayersCount = graph.GetLayerCount();
            var minNodeDegree = graph.Where(n => n.Layer < distinctLayersCount - 1).Min(n => n.Degree);

            Assert.AreEqual(settings.LayerCount, distinctLayersCount);
            Assert.IsTrue(minNodeDegree >= settings.MinNodeDegree);
        }
    }
}
