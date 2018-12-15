using System;
using System.Collections.Generic;

namespace YarD
{
   
    class Program
    {
        static void Main(string[] args)
        {
            int N = 100000;
            int c = 0;
            int maxc = 2;
            var dic = new Dictionary<int, int>();
            for(int i = 2; i < N; i++)
            {
                c = 0;
                for(int j = 1; j <= i; j++)
                {
                    if(i%j == 0)
                    {
                        c ++;
                    }
                }

                if (!dic.ContainsKey(c))
                {
                    dic.Add(c, i);
                    Console.WriteLine(i + " " + c);
                }
            }

            Console.WriteLine("finish");
            Console.ReadLine();
        }
    }
}
