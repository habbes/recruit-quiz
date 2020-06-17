using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace tinker
{
    public static class Util
    {
        public static double Bench(Action func, int iters = 10)
        {
            double total = 0;
            for (int i = 0; i < iters; i++)
            {
                Stopwatch s = new Stopwatch();
                s.Start();
                func();
                s.Stop();
                total += s.Elapsed.Milliseconds;
            }
            Console.WriteLine("Total {0}", total);
            double average = (total / 1000.0) / iters;
            return average;
        }

        public static IList<T> MultiplyList<T>(IList<T> baseList, int multiplier)
        {
            List<T> result = new List<T>(baseList.Count * multiplier);
            for (int i = 0; i < multiplier; i++)
            {
                result.AddRange(baseList);
            }
            Console.WriteLine("new length {0}", result.Count);
            return result;
        }
    }
}