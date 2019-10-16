using System;
using System.Collections.Generic;
using System.Linq;

namespace YarQual2019I
{
    class index
    {
        public int tailPosition;
        public List<int> tailChain = null;
        public int nextTail = -1;
        public int cycleStart = -1;
        public int tailFullLen = -1;
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
            var indexL = new index[N];
            var indexR = new index[N];
            index[] ind;

            for ( int i = 0; i < Q; i++)
            {
                var req = Console.ReadLine().Split(new char[] { ' ' }).Select(Int64.Parse).ToArray<Int64>();

                int station = (int)(req[0] - 1);
                bool coin = req[3] == 0;
                Int64 l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                int[] G = coin ? L : R;
                station = go(station,l, G, ind);
                
                coin = !coin;

                l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                G = coin ? L : R;

                station = go(station, l, G, ind);

                Console.WriteLine(station+1);
            }

            Console.ReadLine();

        }

        public static void calcIndexes(index[] index, int[] G, int i )
        {
            if (index[i] != null)
            {
                return;
            }
            var stack = new Stack<int>();
            var arr = Enumerable.Repeat<int>(-1, G.Length).ToArray();

            int l = 0;
            while (arr[i] == -1 && index[i] == null)
            {
                arr[i] = l;
                stack.Push(i);
                i = G[i];
                l++;
            }

            int tailPosition = 0;
            int nextTail = i;
            List<int> tailChain = null;
            
            index ind = null;

            //build cycle
            if (arr[i] != -1)
            {
                
                tailPosition = l - arr[i] - 1;
                tailChain = new int[tailPosition+1].ToList();
                while (tailPosition >= 0)
                {
                    i = stack.Pop();
                    tailChain[tailPosition] = i;
                    ind = new index();
                    ind.tailPosition = tailPosition;
                    ind.tailChain = tailChain;
                    ind.nextTail = -1;
                    index[i] = ind;
                    tailPosition --;
                }
            }

            //jump to cycle params init
            int cycleStart = -1;
            int tailFullLen = -1;
            if (index[nextTail].nextTail == -1)
            {
                cycleStart = nextTail;
                tailFullLen = 1;
            }
            else
            {
                cycleStart = index[nextTail].cycleStart;
                tailFullLen = index[nextTail].tailFullLen + 1;
            }

            //try continue next tail 
            if ( index[nextTail].nextTail != -1 && index[nextTail].tailChain.Count == index[nextTail].tailPosition + 1)
            {
                tailChain = index[nextTail].tailChain;
                tailPosition = index[nextTail].tailPosition + 1;
                nextTail = index[nextTail].nextTail;
            }
            else
            {
                tailChain = new List<int>();
                tailPosition = 0;
            }

            //build tail
            while (stack.Count > 0)
            {
                i = stack.Pop();
                tailChain.Add(i);
                ind = new index();
                ind.tailPosition = tailPosition;
                ind.tailChain = tailChain;
                ind.nextTail = nextTail;
                ind.cycleStart = cycleStart;
                ind.tailFullLen = tailFullLen;
                index[i] = ind;
                tailPosition++;
                tailFullLen++;
            }
        }

        static int go(int station, Int64 l, int[] G, index[] ind)
        {

            calcIndexes(ind, G, station);

            while (true)
            {
                if (l == 0)
                {
                    return station;
                }

                if(ind[station].nextTail != -1 &&  l >= ind[station].tailFullLen)
                {
                    l -= ind[station].tailFullLen;
                    station = ind[station].cycleStart;
                }

                if (ind[station].nextTail == -1)
                {
                    return ind[station].tailChain[(int)((l + ind[station].tailPosition) % ind[station].tailChain.Count)];
                }

                if (l <= ind[station].tailPosition)
                {
                    return ind[station].tailChain[ind[station].tailPosition - (int)l];
                }

                l -= ind[station].tailPosition + 1;
                station = ind[station].nextTail;
            }

            throw new Exception();
        }

    }
}
