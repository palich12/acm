using System;
using System.Collections.Generic;
using System.Linq;

namespace _1326
{
   class Offer : IComparable
    {
        public int key;
        public int price;

        public int CompareTo(object obj)
        {
            return price.CompareTo(((Offer)obj).price);
        }
    }

    class Program
    {

        static Dictionary<int, int> dic = new Dictionary<int, int>();



        static void Main(string[] args)
        {


            var offers = new List<Offer>();
            int N = Int32.Parse(Console.ReadLine());
            for(int i = 0; i < N; i++)
            {
                offers.Add(new Offer() {
                    key = 1 << i,
                    price = Int32.Parse(Console.ReadLine())
                });
            }

            int M = Int32.Parse(Console.ReadLine());
            int key = 0;
            for (int i=0; i < M; i++)
            {
                var arr = Console.ReadLine().Split(' ').Select(s => Int32.Parse(s)).ToArray();
                key = 0;
                for ( int j = 0; j < arr[1]; j ++ )
                {
                    key += 1 << arr[j + 2]-1;
                }
                offers.Add(new Offer()
                {
                    key = key,
                    price = arr[0]
                });

            }

            var tarr = Console.ReadLine().Split(' ').Select(s => Int32.Parse(s)).ToArray();
            key = 0;
            for (int j = 0; j < tarr[0]; j++)
            {
                key += 1 << tarr[j + 1] - 1;
            }

            offers.Sort();
            Console.WriteLine(req(key, offers));
            Console.ReadKey();
        }

        static int req(int key, List<Offer> offers)
        {
            if (key == 0)
            {
                return 0;
            }


            if (dic.ContainsKey(key))
            {
                return dic[key];
            }


            var newoffers = new List<Offer>();
            foreach (var offer in offers)
            {
                if ((key & ~offer.key) == key)
                    continue;
                newoffers.Add(offer);
            }

            var result = -1;
            foreach (var offer in newoffers)
            {

                if (result != -1 && result <= offer.price)
                {
                    break;
                }

                var newkey = key & ~offer.key;
                var v = req(newkey, newoffers) + offer.price;
                if (result == -1 || result > v)
                {
                    result = v;
                }
            }

            dic[key] = result;
            return result;
        }

    }
}
