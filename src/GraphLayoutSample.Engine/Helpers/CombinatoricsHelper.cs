using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLayoutSample.Engine.Helpers
{
    public static class CombinatoricsHelper
    {
        public static IEnumerable<IReadOnlyList<T>> GetAllPermutations<T>(this IReadOnlyList<T> set)
        {
            var count = set.Count;
            var a = new int[count];
            var p = new int[count];

            var yieldRet = new T[count];

            var list = new List<T>(set);

            int i;

            for (i = 0; i < count; i++)
            {
                a[i] = i + 1;
                p[i] = 0;
            }
            yield return list;
            i = 1;
            while (i < count)
            {
                if (p[i] < i)
                {
                    var j = i % 2 * p[i];
                    var tmp = a[j];
                    a[j] = a[i];
                    a[i] = tmp;

                    for (var x = 0; x < count; x++)
                    {
                        yieldRet[x] = list[a[x] - 1];
                    }
                    yield return yieldRet;

                    p[i]++;
                    i = 1;
                }
                else
                {
                    p[i] = 0;
                    i++;
                }
            }
        }
    }
}
