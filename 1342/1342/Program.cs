using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _1342
{

    class factory
    {
        public int count;
        public double first;
        public double last;
        public int allcount;
    }

    class Program
    {
        public static factory[] factorys;
        public static double[,] costs;

        static void Main(string[] args)
        {


            var line = Console.ReadLine().Split().Select( i => Int32.Parse(i) ).ToArray();
            int N = line[0], M = line[1];
            factorys = new factory[N];

            int allcount = 0;
            for ( int i = 0; i < N; i++)
            {
                line = Console.ReadLine().Split().Select(j => Int32.Parse(j)).ToArray();
                var f = new factory();
                f.count = line[0];
                f.first = line[1];
                f.last = line[2];
                allcount += f.count;
                f.allcount = allcount;
                factorys[i] = f;
            }

            //--------------
            double result = 0;
            int count = 0;
            foreach( var f in factorys)
            {
                count += f.count;
                result += ((f.first + f.last) * f.count) * 0.5;
            }
            if(count < M)
            {
                Console.WriteLine("Maximum possible amount: " + count);
                Console.WriteLine("Minimum possible cost: " + result.ToString("F2", CultureInfo.InvariantCulture));
                //Console.ReadLine();
                return;
            }

            //--------------
            costs = new double[M + 1, N];
            result = req(M, N-1);
            Console.WriteLine("Minimum possible cost: " + result.ToString("F2", CultureInfo.InvariantCulture));
            Console.ReadLine();
        }

        static double req(int count, int factoryid)
        {
            if(count == 0)
            {
                return 0;
            }
            if(factoryid < 0)
            {
                return Double.MaxValue;
            }
            if( costs[count,factoryid] != 0)
            {
                return costs[count, factoryid];
            }
            if ( factoryid == 0 )
            {
                costs[count, 0] = S(count, 0);
                return costs[count, 0];
            }

            double result = Double.MaxValue;
            int min = Math.Max(0,count - factorys[factoryid - 1].allcount);
            int max = Math.Min(count, factorys[factoryid].count);
            for (int i = 0; i <= max; i++)
            {
                var d = S(i, factoryid);
                if (d == Double.MaxValue)
                {
                    continue;
                }
                double prev = req(count-i,factoryid-1);
                if (prev == Double.MaxValue)
                {
                    continue;
                }

                if( prev + d < result)
                {
                    result = prev + d;
                }
            }

            costs[count, factoryid] = result;
            return result;
        }

        static double S(int count, int factoryid)
        {
            
            if (factorys[factoryid].count < count)
            {
                return Double.MaxValue;
            }

            double d = 0;
            if (factorys[factoryid].count > 1)
            {
                d = (factorys[factoryid].last - factorys[factoryid].first) / (factorys[factoryid].count - 1.0);
            }
            return (2.0 * factorys[factoryid].first + d * (count - 1)) * count * 0.5;
        }
    }
}
