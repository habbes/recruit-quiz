using System;
using System.Collections.Generic;
using System.Linq;

namespace tinker
{
    public static class BlockTest
    {
        public static int Blocks(int[] blocks)
        {
            int maxDist = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                int dist = CalcDistance(blocks, i);
                maxDist = dist > maxDist ? dist : maxDist;
            }

            return maxDist;
        }

        public static int Blocks2(int[] blocks)
        {
            int maxDist = 0;
            List<int> candidates = new List<int>();
            candidates.Add(0);
            int lastDir = 1;
            for (int i = 1; i < blocks.Length; i++)
            {
                int dir = blocks[i] < blocks[i - 1] ? dir = -1 : dir = 1;
                if (dir < lastDir)
                {
                    // we've crossed a local maximum
                    // we've reached a peak and should start counting distance a fresh
                }

                if (dir > lastDir)
                {
                    // we've crossed a local minimum
                    candidates.Add(i - 1);
                }

                if (i == blocks.Length - 1) candidates.Add(i);

                lastDir = dir;
            }

            foreach (int c in candidates)
            {
                // Console.WriteLine("Candidate {0}", c);
                int dist = CalcDistance(blocks, c);
                maxDist = dist > maxDist ? dist : maxDist;
            }

            return maxDist;
        }

        public static int Blocks3(int [] blocks)
        {
            int maxDist = 1;
            int lastDir = 1;
            int peakCount = 1;
            int troughCount = 1;
            for (int i = 1; i < blocks.Length; i++)
            {
                int dir = blocks[i] < blocks[i - 1] ? dir = -1 : dir = 1;
                if (dir < lastDir)
                {
                    // we've crossed a local maximum
                    // we've reached a peak and should start counting distance a fresh
                    if (peakCount > maxDist) maxDist = peakCount;
                    peakCount = 1;
                    troughCount += 1;
                }
                else if (dir > lastDir)
                {
                    if (troughCount > maxDist) maxDist = troughCount;
                    troughCount = 1;
                    peakCount += 1;
                }
                else
                {
                    troughCount += 1;
                    peakCount += 1;
                }

                lastDir = dir;
            }

            // foreach (int c in candidates)
            // {
            //     // Console.WriteLine("Candidate {0}", c);
            //     int dist = CalcDistance(blocks, c);
            //     maxDist = dist > maxDist ? dist : maxDist;
            // }

            maxDist = peakCount > maxDist ? peakCount : maxDist;
            maxDist = troughCount > maxDist ? troughCount: maxDist;

            return maxDist;
        }

        public static int CalcDistance(int[] blocks, int start)
        {
            int left = start;
            int right = start;

            while (left > 0 && blocks[left - 1] >= blocks[left]) left--;
            while ((right + 1) < blocks.Length && blocks[right + 1] >= blocks[right]) right++;

            return (right - left) + 1;
        }
        public static void TestBlocks()
        {
            Console.WriteLine("{0}, {1}", Blocks3(new int[] { 2, 6, 8, 5}), 3);
            Console.WriteLine("{0}, {1}", Blocks3(new int[] { 1, 5, 5, 2, 6}), 4);
            Console.WriteLine("{0}, {1}", Blocks3(new int[] { 1, 1 }), 2);
            Console.WriteLine("{0}, {1}", Blocks3(new int[] { 3, 2, 1, 2, 3 }), 5);
            Console.WriteLine("{0}, {1}", Blocks3(new int[] { 5, 4, 2, 1 }), 4 );
            Console.WriteLine("{0}, {1}", Blocks3(new int[] { 3, 5, 4, 2, 1 }), 4 );
            
            var bigList = Util.MultiplyList(new int[] { 3, 2, 1, 2, 3 }, 40000);
            var time = Util.Bench(() => Blocks3(bigList.ToArray()));
            Console.WriteLine("time {0}", time);
        }
    }
}