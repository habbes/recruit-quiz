using System;
using System.Collections.Generic;
using System.Linq;
using static tinker.Util;

namespace tinker
{
    public static class RankTest
    {
        static int Ranks(int[] ranks)
        {
            var supervisors = new HashSet<int>();
            var reports = new Dictionary<int, int>();
            var count = 0;

            foreach (var rank in ranks)
            {
                supervisors.Add(rank);
                var supervisor = rank + 1;

                if (reports.ContainsKey(supervisor))
                {
                    reports[supervisor] += 1;
                }
                else
                {
                    reports[supervisor] = 1;
                }
            }

            foreach (var s in supervisors)
            {
                if (reports.TryGetValue(s, out int n))
                {
                    count += n;
                }
            }

            return count;
        }

        public static int Ranks2(int [] ranks)
        {
            var count = 0;
            var reports = new HashSet<int>();
            var pendingSupervisors = new Dictionary<int, int>();
            foreach(var rank in ranks)
            {
                var supervisor = rank + 1;
                var report = rank - 1;
                reports.Add(report);

                if (reports.Contains(rank))
                {
                    count += 1;
                }

                int numReports;

                if (pendingSupervisors.TryGetValue(rank, out numReports) && numReports > 0)
                {
                    count += pendingSupervisors[rank];
                }
                

                if (pendingSupervisors.TryGetValue(supervisor, out numReports))
                {
                    if (numReports > 0) pendingSupervisors[supervisor] = numReports + 1;
                }
                else
                {
                    pendingSupervisors[supervisor] = 1;
                }

                pendingSupervisors[rank] = -1;
            }

            return count;
        }

        public static void TestRank()
        {
            Console.WriteLine("{0}, {1}", Ranks(new [] {3, 4, 3, 0, 2, 2, 3, 0, 0, 4}), 5);
            Console.WriteLine("{0}, {1}", Ranks(new [] {0, 0, 0, 0, 0, 1}), 5);
            Console.WriteLine("{0}, {1}", Ranks(new [] {1, 0, 0, 0}), 3);
            Console.WriteLine("{0}, {1}", Ranks(new [] {0, 0, 0, 1, 0, 0, 0}), 6);
            Console.WriteLine("{0}, {1}", Ranks(new [] {1, 0, 5, 1, 3}), 1);

            int[] bigList = MultiplyList(new int[] { 0, 1 }, 100000).ToArray();
            double time = Bench(() => Ranks(bigList));
            Console.WriteLine("Avg. Time {0}", time);
        }
    }
}