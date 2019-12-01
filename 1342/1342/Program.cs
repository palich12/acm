using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace _1342
{

    class titem
    {
        public double price = 0;
        public int count = 0;
        public List<factory> factorys = new List<factory>();
    }

    class factory
    {
        public int count;
        public double first;
        public double last;
    }

    class Program
    {
        public static factory[] factorys;

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
            var items = new List<titem>();
            items.Add( new titem());
            result = double.MaxValue;
            foreach (var factory in factorys)
            {
                
                double price = ((factory.first + factory.last) * factory.count) * 0.5;
                var newitems = new List<titem>();
                foreach ( var item in items )
                {
                    if (factory.count + item.count < M)
                    {
                        var newitem = new titem()
                        {
                            count = factory.count + item.count,
                            price = price + item.price,
                            factorys = item.factorys.ToList()
                        };
                        newitem.factorys.Add(factory);
                        newitems.Add(newitem);
                    }
                }
                items.AddRange(newitems);
            }

            foreach(var factory in factorys)
            {
                foreach (var item in items)
                {
                    if ( factory.count >= M - item.count &&
                         !item.factorys.Contains(factory))
                    {
                        double d = 0;
                        if( factory.count > 1)
                        {
                            d = (factory.last - factory.first) / (factory.count - 1);
                        }
                        double price = (2*factory.first + d*(M - item.count - 1))* (M - item.count)*0.5 + item.price;
                        if(result > price)
                        {
                            result = price;
                        }
                    }
                }
            }

            Console.WriteLine("Minimum possible cost: " + result.ToString("F2", CultureInfo.InvariantCulture));
            Console.ReadLine();
        }
    }
}
