using System;

namespace YarQual2019J
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = Console.ReadLine().Split(new char[] { ' ' });

            long N = long.Parse(s[0]);
            long M = long.Parse(s[1]);
            long k = long.Parse(s[2]);
            long p = 0;
            long i = 0;

            while (k > 0)
            {
                long v = i % 2 == 0 ? 1 : -1;
                long d = M - i - (i == 0 ? 1 : 0);
                if(d <= 0)
                {
                    break;
                }

                if(d <= k )
                {
                    p += d*v;
                    k -= d;
                }
                else
                {
                    p += k * v;
                    break;
                }

                d = N - i - 1;
                if (d <= 0)
                {
                    break;
                }

                if (d <= k)
                {
                    p += d * v;
                    k -= d;
                }
                else
                {
                    p += k * v;
                    break;
                }

                i++;
            }

            Console.WriteLine(p % 7 + 1);
            Console.ReadLine();
        }
    }
}
