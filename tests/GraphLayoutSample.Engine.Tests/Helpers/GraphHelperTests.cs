using System;
using System.Collections.Generic;
using GraphLayoutSample.Engine.Helpers;
using GraphLayoutSample.Engine.Models;
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
    }
}
