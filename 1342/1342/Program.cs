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
            
            for( int i = 0; i < N; i++)
            {
                line = Console.ReadLine().Split().Select(j => Int32.Parse(j)).ToArray();
                var f = new factory();
                f.count = line[0];
                f.first = line[1];
                f.last = line[2];
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
            for (int i = 0; i <= M; i++)
            //{
            //    for (int j = 0; j < N; j++)
            //    {
            //        costs[i, j] = -1;
            //    }
            //}
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

            double result = Double.MaxValue;
            for (int i = 0; i <= count; i++)
            {
                var d = S(count-i, factoryid);
                if (d == Double.MaxValue)
                {
                    continue;
                }
                double prev = req(i,factoryid-1);
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
