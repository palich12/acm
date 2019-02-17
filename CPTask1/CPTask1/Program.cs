using System;

namespace CPTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            int res = 0;
            for(int i = 0; i < 10000; i++)
            {
                int a = i / 1000;
                int b = (i / 100) % 10;
                int c = (i / 10) % 10;
                int d = i % 10;

                if(    Math.Abs(a - b) > 1
                    && Math.Abs(b - c) > 1
                    && Math.Abs(c - d) > 1)
                {
                    res++;
                }
            }
            Console.WriteLine(res.ToString());
            Console.ReadLine();
        }
    }
}
