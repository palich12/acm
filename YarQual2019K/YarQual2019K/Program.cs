using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {

        static void Main(string[] args)
        {


            var q = Int32.Parse(Console.ReadLine());
            var Q = new int[q];
            for (int i = 0; i < q; i++)
            {
                Q[i] = Int32.Parse(Console.ReadLine());
            }

            var N = new int[Q.Max() + 1];
            N[1] = 1;
            var queue = new Queue<int>();
            queue.Enqueue(1);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();

                if (n + 1 < N.Length && (N[n + 1] == 0 || N[n + 1] > N[n] + 1))
                {
                    N[n + 1] = N[n] + 1;
                    queue.Enqueue(n + 1);
                }

                int a = Math.Min(n, (int)((N.Length - 1) / n));
                while (a > 1)
                {
                    if (N[a * n] == 0 || N[a * n] > N[n] + 1)
                    {
                        N[a * n] = N[n] + 1;
                        queue.Enqueue(a * n);
                    }
                    a--;
                }
            }


            foreach (var n in Q)
            {
                Console.WriteLine(N[n].ToString());
            }

            //Console.ReadLine();
        }
    }
}
