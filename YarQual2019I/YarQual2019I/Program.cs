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

        public List<int> tailChain = null;
        public int nextTail = -1;
    }

    class index2
    {
        public int tailPosition;
        public List<int> tailChain = null;
        public int nextTail = -1;
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
            var indexL = new index2[N];
            var indexR = new index2[N];
            index2[] ind;

            for ( int i = 0; i < Q; i++)
            {
                var req = Console.ReadLine().Split(new char[] { ' ' }).Select(Int64.Parse).ToArray<Int64>();

                int station = (int)(req[0] - 1);
                bool coin = req[3] == 0;
                Int64 l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                int[] G = coin ? L : R;
                station = go2(station,l, G, ind);
                
                coin = !coin;

                l = coin ? req[1] : req[2];
                ind = coin ? indexL : indexR;
                G = coin ? L : R;

                station = go2(station, l, G, ind);

                Console.WriteLine(station+1);
            }

            Console.ReadLine();

        }

        static void calcIndexes(index[] index, int[] G, int j)
        {
            var arr = new int[G.Length];

            if (index[j] != null)
            {
                return;
            }
            var stack = new Stack<int>();

            arr = Enumerable.Repeat<int>(-1, G.Length).ToArray();
            var l = 0;
            while (arr[j] == -1 && index[j] == null)
            {
                arr[j] = l;
                stack.Push(j);
                j = G[j];
                l++;
            }

            int cycleStart = j;
            int cycleLength = l - arr[j];
            int tailLength = 0;
            bool inCycle = true;

            int cyclePosition = -1;
            int[] cycleChain = null;
            var tailChain = new List<int>();
            int nextTail = -1;
            

            if (index[j] != null)
            {
                cycleStart = index[j].cycleStart;
                cycleLength = index[j].cycleLength;
                inCycle = false;

                if(index[j].tailChain == null)
                {
                    tailLength = 0;
                    tailChain = new List<int>();
                }
                else if ( index[j].tailChain.Count == index[j].tailLength )
                {
                    tailLength = index[j].tailLength;
                    tailChain = index[j].tailChain;                
                }
                else
                {
                    nextTail = j;
                    tailLength = 0;
                    tailChain = new List<int>();
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
                ind.nextTail = nextTail;

                if (inCycle)
                {
                    cycleChain[cyclePosition] = j;
                    ind.cycleChain = cycleChain;
                    ind.cyclePosition = cyclePosition;
                    cyclePosition--;
                }
                else
                {
                    tailChain.Add(j);
                    ind.tailChain = tailChain;
                }


                index[j] = ind;
                if (cyclePosition == -1 && inCycle)
                {
                    inCycle = false;
                    tailChain = new List<int>();
                }
            }
            
        }

        static int go(int station, Int64 l, int[] G, index[] ind)
        {

            calcIndexes(ind, G, station);

            bool tailFinished = false;
            while (!tailFinished)
            {
                if (l == 0)
                {
                    return station;
                }

                if (l < ind[station].tailLength)
                {
                    return ind[station].tailChain[ind[station].tailLength - (int)l - 1];
                }

                l -= ind[station].tailLength;
                if ( ind[station].nextTail >=0)
                {
                    station = ind[station].nextTail;
                }
                else
                {
                    tailFinished = true;
                    station = ind[station].cycleStart;
                }

            }

            return ind[station].cycleChain[(l + ind[station].cyclePosition) % ind[station].cycleLength];
        }


        public static void calcIndexes2(index2[] index, int[] G, int i )
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
            index2 ind = null;
            if (arr[i] != -1)
            {
                
                tailPosition = l - arr[i] - 1;
                tailChain = new int[tailPosition+1].ToList();
                while (tailPosition >= 0)
                {
                    i = stack.Pop();
                    tailChain[tailPosition] = i;
                    ind = new index2();
                    ind.tailPosition = tailPosition;
                    ind.tailChain = tailChain;
                    ind.nextTail = -1;
                    index[i] = ind;
                    tailPosition --;
                }

                return;

            }


            tailChain = new List<int>();
            tailPosition = 0;

            while (stack.Count > 0)
            {
                i = stack.Pop();
                tailChain.Add(i);
                ind = new index2();
                ind.tailPosition = tailPosition;
                ind.tailChain = tailChain;
                ind.nextTail = nextTail;
                index[i] = ind;
                tailPosition++;
            }
        }

        static int go2(int station, Int64 l, int[] G, index2[] ind)
        {

            calcIndexes2(ind, G, station);

            while (true)
            {
                if (l == 0)
                {
                    return station;
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
