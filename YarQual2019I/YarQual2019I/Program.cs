using System;
using System.Collections.Generic;
using System.Linq;

namespace YarQual2019I
{
    class index
    {
        public int tailLength { get; set; }
        public int cycleLength { get; set; }
        public int cycleStart { get; set; }
    }

    class Program
    {

       
        static void Main(string[] args)
        {

            var qn = Console.ReadLine().Split(new char[] { ' ' }).Select(int.Parse).ToArray<int>();
            int N = qn[0];
            int Q = qn[1];
            int[] L = Console.ReadLine().Split(new char[] { ' ' }).Select(s => int.Parse(s) - 1).ToArray<int>();
            int[] R = Console.ReadLine().Split(new char[] { ' ' }).Select(s => int.Parse(s) - 1).ToArray<int>();
            index[] indexL = new index[N];
            index[] indexR = new index[N];

            //calcIndexes(indexL, L);
            //calcIndexes(indexR, R);

            for ( int i = 0; i < Q; i++)
            {
                var req = Console.ReadLine().Split(new char[] { ' ' }).Select(int.Parse).ToArray<int>();

                int station = req[0] - 1;
                int c = req[3];
                int l;
                index[] ind;
                int[] G;
                bool coin = c == 0;

                l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                G = coin ? L : R;

                calcIndexes(ind, G, station);

                if ( l >= ind[station].tailLength)
                {
                    l -= ind[station].tailLength;
                    l %= ind[station].cycleLength;
                    station = ind[station].cycleStart;
                }
                while ( l > 0)
                {
                    station = G[station];
                    l--;
                }

                coin = !coin;

                l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                G = coin ? L : R;

                calcIndexes(ind, G, station);

                if (l >= ind[station].tailLength)
                {
                    l -= ind[station].tailLength;
                    l %= ind[station].cycleLength;
                    station = ind[station].cycleStart;
                }
                while (l > 0)
                {
                    station = G[station];
                    l--;
                }

                Console.WriteLine(station+1);
            }

            Console.ReadLine();

        }

        static void calcIndexes(index[] index, int[] G, int i)
        {
            var arr = new int[G.Length];

            if (index[i] != null)
            {
                return;
            }
            var stack = new Stack<int>();

            arr = Enumerable.Repeat<int>(-1, G.Length).ToArray();
            int j = i;
            var l = 0;
            while (arr[j] == -1 && index[j] == null)
            {
                arr[j] = l;
                stack.Push(j);
                j = G[j];
                l++;
            }

            int cycleStart = arr[j];
            int cycleLength = l - arr[j];
            int tailLength = 0;
            bool inCycle = true;
            if (index[j] != null)
            {
                cycleStart = index[j].cycleStart;
                cycleLength = index[j].cycleLength;
                tailLength = index[j].tailLength;
                inCycle = false;
            }

            while (stack.Count > 0)
            {
                j = stack.Pop();
                var ind = new index();

                ind.cycleLength = cycleLength;

                cycleStart = inCycle ? j : cycleStart;
                ind.cycleStart = cycleStart;

                tailLength = inCycle ? 0 : tailLength + 1;
                ind.tailLength = tailLength;


                index[j] = ind;
                if (j == cycleStart)
                {
                    inCycle = false;
                }
            }
            
        }
    }
}
