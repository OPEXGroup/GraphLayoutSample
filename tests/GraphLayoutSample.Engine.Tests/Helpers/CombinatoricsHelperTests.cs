using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GraphLayoutSample.Engine.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphLayoutSample.Engine.Tests.Helpers
{
    [TestClass]
    public class CombinatoricsHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void GetAllPermutations_ThrowsOnNullArguments()
        {
            foreach (var list in CombinatoricsHelper.GetAllPermutations<object>(null))
            {
                
            }
        }

        [TestMethod]
        public void GetAllPermutations_ReturnsCorrectNumberOfElements()
        {
            var list = new List<int> {1};
            Assert.AreEqual(1, list.GetAllPermutations().Count());

            list.Add(2);
            Assert.AreEqual(2, list.GetAllPermutations().Count());

            list.Add(3);
            Assert.AreEqual(6, list.GetAllPermutations().Count());

            list.Add(4);
            Assert.AreEqual(24, list.GetAllPermutations().Count());

            list.Add(5);
            Assert.AreEqual(120, list.GetAllPermutations().Count());

            list.Add(6);
            Assert.AreEqual(720, list.GetAllPermutations().Count());
        }

        [TestMethod]
        public void GetAllPermutations_ReturnsEqualSetsForAllIterations()
        {
            var list = new List<int> {1, 2, 3, 4, 5};
            IReadOnlyList<int> prevPermutation = null;
            foreach (var permutation in list.GetAllPermutations())
            {
                if (prevPermutation != null)
                    Assert.IsTrue(AreSameSets(permutation, prevPermutation));
                prevPermutation = permutation;
            }
        }

        [TestMethod]
        public void GetAllPermutations_ReturnsDifferentOrderedSetsForAllIterations()
        {
            var list = new List<int> { 1, 2, 3, 4 };
            var prevPermutations = new List<IReadOnlyList<int>>();
            foreach (var permutation in list.GetAllPermutations())
            {
                foreach (var prevPermutation in prevPermutations)
                {
                    Assert.IsFalse(AreSameOrderedSets(permutation, prevPermutation));
                }
                    
                prevPermutations.Add(permutation.ToList());
            }
        }

        private static bool AreSameSets(IReadOnlyCollection<int> firstList, IReadOnlyCollection<int> secondList)
        {
            if (firstList.Count != secondList.Count)
                return false;

            var orderedFirstList = firstList.OrderBy(x => x).ToList();
            var orderedSecondList = secondList.OrderBy(x => x).ToList();
            var count = firstList.Count;

            for (var i = 0; i < count; ++i)
            {
                if (orderedFirstList[i] != orderedSecondList[i])
                    return false;
            }

            return true;
        }

        private static bool AreSameOrderedSets(IReadOnlyList<int> firstList, IReadOnlyList<int> secondList)
        {
            if (firstList.Count != secondList.Count)
                return false;

            var count = firstList.Count;
            for (var i = 0; i < count; ++i)
            {
                if (firstList[i] != secondList[i])
                    return false;
            }

            return true;
        }
    }
}
