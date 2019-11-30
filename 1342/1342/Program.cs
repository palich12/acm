using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _1342
{

    class titem
    {
        public double price = 0;
        public List<int> factorys = new List<int>();
    }

    class factory
    {
        public int Count;
        public double first;
        public double last;
    }

    class Program
    {
        public static titem[,] table;
        public static factory[] factorys;

        static void Main(string[] args)
        {


            var line = Console.ReadLine().Split().Select( i => Int32.Parse(i) ).ToArray();
            int N = line[0], M = line[1];
            factorys = new factory[N];
            table = new titem[N, M + 1];
            for( int i = 0; i < N; i++)
            {
                line = Console.ReadLine().Split().Select(j => Int32.Parse(j)).ToArray();
                var f = new factory();
                f.Count = line[0];
                f.first = line[1];
                f.last = line[2];
                factorys[i] = f;
            }

            //--------------
            double result = 0;
            int count = 0;
            foreach( var f in factorys)
            {
                count += f.Count;
                result += ((f.first + f.last) * f.Count) * 0.5;
            }
            if(count < M)
            {
                Console.WriteLine("Maximum possible amount: " + count);
                Console.WriteLine("Minimum possible cost: " + result.ToString("F2", CultureInfo.InvariantCulture));
                //Console.ReadLine();
                return;
            }

            //--------------
            table[0, 0] = new titem();
            if ( factorys[0].Count <= M)
            {
                var item = new titem();
                item.price = ((factorys[0].first + factorys[0].last) * factorys[0].Count) * 0.5;
                item.factorys.Add(0);
                table[0, factorys[0].Count] = item;
            }
            for(int i = 1; i < N; i++)
            {
                for(int j = 0; j <= M; j++)
                {
                    if( table[i-1,j] != null)
                    {
                        table[i, j] = table[i - 1, j];
                    }
                }

                double val = ((factorys[i].first + factorys[i].last) * factorys[i].Count) * 0.5;
                for (int j = 0; j <= M; j++)
                {
                    var newindex = j + factorys[i].Count;
                    if (
                        table[i, j] != null &&
                        newindex <= M)
                    {
                        if( table[i, newindex] == null || val + table[i, j].price < table[i, newindex].price)
                        {
                            var item = new titem();
                            item.price = val + table[i, j].price;
                            item.factorys = table[i, j].factorys.ToList();
                            item.factorys.Add(i);
                            table[i, newindex] = item;
                        }
                        
                    }
                }
            }

            result = double.MaxValue;
            if(table[N-1,M] != null)
            {
                result = table[N - 1, M].price;
            }

            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j <= M; j++)
                {
                    if (table[N - 1, j] != null &&
                        factorys[i].Count >= M - j &&
                        !table[N-1,j].factorys.Contains(i))
                    {
                        double val = (factorys[i].first + factorys[i].last) * 0.5 * (M - j) + table[N - 1, j].price;
                        if(result > val)
                        {
                            result = val;
                        }
                    }
                }
            }

            Console.WriteLine("Minimum possible cost: " + result.ToString("F2", CultureInfo.InvariantCulture));
            Console.ReadLine();
        }
    }
}
