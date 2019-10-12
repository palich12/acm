using System;
using System.Collections.Generic;
using System.Linq;

namespace YarQual2019I
{
    class index
    {
        public int tailLength;
        public int cycleLength;
        public int cycleStart;

        public int[] cycleChain;
        public int cyclePosition;

        public List<int> tailChain = new List<int>();
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
                station = go(station,l, G, ind);
                
                coin = !coin;

                l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                G = coin ? L : R;

                calcIndexes(ind, G, station);
                station = go(station, l, G, ind);

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

            int cyclePosition = -1;
            int[] cycleChain = null;
            List<int> tailCain = null;
            

            if (index[j] != null)
            {
                cycleStart = index[j].cycleStart;
                cycleLength = index[j].cycleLength;
                tailLength = index[j].tailLength;
                inCycle = false;

                if ( index[j].tailChain.Count == index[j].tailLength )
                {
                    tailCain = index[j].tailChain;
                }
                else
                {
                    tailCain = index[j].tailChain.GetRange(0, index[j].tailLength);
                }
            }
            else
            {
                cyclePosition = cycleLength - 1;
                cycleChain = new int[cycleLength];
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

                if (cyclePosition >= 0)
                {
                    cycleChain[cyclePosition] = j;
                    ind.cycleChain = cycleChain;
                    ind.cyclePosition = cyclePosition;
                    cyclePosition--;
                }
                else
                {
                    tailCain.Add(j);
                    ind.tailChain = tailCain;
                }


                index[j] = ind;
                if (cyclePosition == -1 && inCycle)
                {
                    inCycle = false;
                    tailCain = new List<int>();
                }
            }
            
        }

        static int go(int station, int l, int[] G, index[] ind)
        {


            calcIndexes(ind, G, station);

            if (l >= ind[station].tailLength)
            {
                l -= ind[station].tailLength;
                station = ind[station].cycleStart;

                l = (l + ind[station].cyclePosition) % ind[station].cycleLength;
                station = ind[station].cycleChain[l];
            }
            else
            {
                l = ind[station].tailLength - l - 1;
                station = ind[station].tailChain[l];
            }

            return station;
        }

    }
}
