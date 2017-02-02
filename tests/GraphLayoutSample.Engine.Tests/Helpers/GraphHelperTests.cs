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
    }
}
