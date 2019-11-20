using System;
using System.Collections.Generic;
using System.Linq;

namespace _1389
{
    class Road
    {
        public int A;
        public int B;
    }

    class Program
    {

        static List<Road>[] Citys;
        static Dictionary<int, List<Road>> dic = new Dictionary<int, List<Road>>();

        static void Main(string[] args)
        {
            int N = Int32.Parse(Console.ReadLine().Split(' ')[0]);
            Citys = new List<Road>[N];
            var Roads = new List<Road>();
            int i;
            for (i = 0; i < N; i++)
            {
                Citys[i] = new List<Road>();
            }
            for(i = 0; i < N - 1; i++)
            {
                var l = Console.ReadLine().Split(' ').Select(s => Int32.Parse(s)).ToArray();

                var road = new Road()
                {
                    A = l[0] - 1,
                    B = l[1] - 1,
                };

                Citys[road.A].Add(road);
                Citys[road.B].Add(road);

            }

            //--------------------------------------------------------

            var queue = new Queue<int>();
            for( i = 0; i < Citys.Length; i ++)
            {
                if(Citys[i].Count == 1)
                {
                    queue.Enqueue(i);
                }
            }

            while(queue.Count > 0)
            {
                var id = queue.Dequeue();
                var city = Citys[id];
                if (city.Count == 0)
                {
                    continue;
                }

                var road = city.First();
                city.Clear();
                Roads.Add(road);
                var nextCityId = road.A != id ? road.A : road.B;
                var nextCity = Citys[nextCityId];
                foreach(var r in nextCity)
                {
                    var nextNextCityId = r.A != nextCityId ? r.A : r.B;
                    var nextNextCity = Citys[nextNextCityId];
                    nextNextCity.Remove(r);
                    if (nextNextCity.Count == 1)
                    {
                        queue.Enqueue(nextNextCityId);
                    }
                }
                nextCity.Clear();
            }

            Console.WriteLine(Roads.Count);
            foreach ( var r in Roads )
            {
                Console.WriteLine((r.A+1) + " " + (r.B+1));
            }

            Console.ReadKey();
        }
    }
}
