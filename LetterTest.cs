using System;
using System.Collections.Generic;
using System.Linq;

namespace tinker
{
    public class LettersTest
    {
        // public static int Letters(string S)
        // {
        //     int totalDeleted = 0;
        //     var reader = new SeqReader(S);
        //     Seq seq = reader.ReadNext();
        //     Seq nextSeq;
        //     // Console.WriteLine("Orig {0}", S);
        //     while ((nextSeq = reader.ReadNext()) != null)
        //     {
        //         // Console.WriteLine($"Merged {seq}, next: {nextSeq}");
        //         seq.MergeWith(nextSeq, out var deleted);
        //         totalDeleted += deleted;
        //     }

        //     // Console.WriteLine($"Final: {seq}");
        //     return totalDeleted;
        // }
        public static int Letters2(string S)
        {
            var reader = new SeqReader(S);
            Seq seq = reader.ReadNext();
            return Letters2Rec(S.Substring(reader.CurrentPos), seq);
        }
        public static int Letters2Rec(string S, Seq seq)
        {
            var reader = new SeqReader(S);
            // Seq seq = reader.ReadNext();
            if (seq == null) return 0;
            Seq nextSeq = reader.ReadNext();
            if (nextSeq == null) return 0;

            Seq opt1 = seq.Clone();
            Seq opt2 = seq.Clone();

            int deleted1;
            int deleted2;
            opt1.MergeWith(nextSeq, 'A', out deleted1);
            opt2.MergeWith(nextSeq, 'B', out deleted2);

            int total1 = deleted1 + Letters2Rec(S.Substring(reader.CurrentPos), opt1);
            int total2 = deleted2 + Letters2Rec(S.Substring(reader.CurrentPos), opt2);
            // Console.WriteLine(seq);
            // Console.WriteLine(opt1);
            // Console.WriteLine(opt2);
            // Console.WriteLine();
            return total1 < total2 ? total1: total2;
            
        }

        public static void TestLetters()
        {
            Console.WriteLine("{0}, {1}", Letters2("BAABBA"), 2);
            Console.WriteLine("{0}, {1}", Letters2("BBABAA"), 3);
            Console.WriteLine("{0}, {1}", Letters2("AABBB"), 0);
            Console.WriteLine("{0}, {1}", Letters2("BBB"), 0);
            Console.WriteLine("{0}, {1}", Letters2("BBBA"), 1);
            Console.WriteLine("{0}, {1}", Letters2("BAABBB"), 1);
            Console.WriteLine("{0}, {1}", Letters2("BAABBB"), 1);
            Console.WriteLine("{0}, {1}", Letters2("BA"), 1);
            Console.WriteLine("{0}, {1}", Letters2("BAAABABAA"), 3);
            Console.WriteLine("{0}, {1}", Letters2("BAAABABABABAA"), 5);
            Console.WriteLine("{0}, {1}", Letters2("BBBBAAABAAA"), 5);
            Console.WriteLine("{0}, {1}", Letters2("AAAABBBABBB"), 1);
            Console.WriteLine("{0}, {1}", Letters2("BBBBAAABAAABBBBBBBBBB"), 5);
            Console.WriteLine("{0}, {1}", Letters2("AAABBBBAA"), 2);
            Console.WriteLine("{0}, {1}", Letters2("BAAABBBABAA"), 4);
            Console.WriteLine("{0}, {1}", Letters2("AAABAAABAAAB"), 2);

            // var bigList = Util.MultiplyList(new char[] { 'A', 'A', 'A', 'B' }, 50000);
            // var bigString = new string(bigList.ToArray());
            // var time = Util.Bench(() => Letters2(bigString));
            // Console.WriteLine("time {0}", time);
        }
    }


    public class SeqReader
    {

        public SeqReader(string text)
        {
            Text = text;
            CurrentPos = 0;
        }

        public string Text;
        public int CurrentPos;

        public bool Finished { get { return CurrentPos >= Text.Length; }}

        public Seq ReadNext()
        {
            var seq = ReadSeq(Text, CurrentPos, out var end);
            CurrentPos = end;
            return seq;
        }

        public static Seq ReadSeq(string s, int start, out int end)
        {
            end = start;
            if (start >= s.Length) return null;

            var seq = new Seq(s[start], start);
            for (int i = start + 1; i < s.Length; i++)
            {
                if (!seq.AddChar(s[i]))
                {
                    end = i;
                    return seq;
                }
            }

            end = s.Length;
            return seq;
        }
    }

    public class Seq
    {
        public int Start;
        public int Stop;
        public char First;
        public char Last;
        public int NumA;
        public int NumB;

        public Seq(char first, int start)
        {
            First = first;
            Last = first;
            Start = start;
            Stop = start;
            NumA = first == 'A' ? 1 : 0;
            NumB = first == 'B' ? 1 : 0;
        }

        public Seq Clone()
        {
            Seq clone = new Seq(First, Start);
            clone.Last = Last;
            clone.Stop = Stop;
            clone.NumA = NumA;
            clone.NumB = NumB;
            return clone;
        }

        public bool AddChar(char c)
        {
            // A cannot follow B in a valid sequence
            if (c == 'A' && Last == 'B') return false;

            if (c == 'B')
            {
                NumB += 1;
            }
            else
            {
                NumA += 1;
            }

            Last = c;
            Stop += 1;
            return true;
        }

        public void MergeWith(Seq other, char toDel, out int deleted)
        {
            deleted = 0;

            // assumes the sequences cannot be directly combined to form a valid seq
            // i.e. this ends with B and other starts with A
            
            // if (NumB < other.NumA)
            if (toDel == 'B')
            {
                // delete this B's
                deleted = NumB;
                NumB = other.NumB;
                NumA += other.NumA;
                Stop = other.Stop;
                Last = other.Last;
                if (First == 'B')
                {
                    First = other.First;
                    Start = other.Start;
                }
            }
            else
            {
                // delete other A's
                deleted = other.NumA;
                NumB += other.NumB;
                Stop = other.Stop;
                if (other.Last != 'A')
                {
                    Last = other.Last;
                }
            }

        }

        public override string ToString()
        {
            string s = "";
            int firstCount, lastCount;
            if (First == 'A')
            {
                firstCount = NumA;
                lastCount = NumB;
            }
            else
            {
                firstCount = NumB;
                lastCount = NumA;
            }

            for (int i = 0; i < firstCount; i++)
            {
                s += First;
            }
            
            for (int i = 0; i < lastCount; i++)
            {
                s += Last;
            }

            return s;
        }
    }
}