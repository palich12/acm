using System;
using System.Collections;
using System.Collections.Generic;

namespace TulaI
{
    class Program
    {
        public static int[,] Matrix;
        public static int N;
        public static Dictionary<string, int> cash;

        static void Main(string[] args)
        {
            N = Int32.Parse(Console.ReadLine());
            Matrix = new int[N,N];
            cash = new Dictionary<string, int>();

            for(int i = 0; i < N; i++)
            {
                var l = Console.ReadLine().Split(new char[] { ' ' });
                for(int j=0; j < N; j++)
                {
                    Matrix[i, j] = Int32.Parse(l[j]);
                }
            }

            Console.WriteLine(req(new List<int>()));
            //Console.ReadLine();
            

        }

        static int req( List<int> path )
        {

            var hash = GetHash(path);
            if ( cash.ContainsKey(hash) )
            {
                return cash[hash];
            }
            if(N - path.Count <= 3)
            {
                cash.Add(hash, 0);
                return 0;
            }

            var res = Int32.MaxValue;
            for (int i = 0; i < N; i++)
            {
                if (!path.Contains(i))
                {
                    int index = -1;
                    for (int j1 = i+1; j1 < N + 2; j1++)
                    {
                        int j = j1 % N;
                        if (!path.Contains(j))
                        {
                            if (index == -1)
                            {
                                index = j;
                            }
                            else
                            {
                                if (Matrix[i, j] >= res)
                                {
                                    break;
                                }
                                var newpath = new List<int>(path);

                                int k = 0;
                                foreach (var p in newpath)
                                {
                                    if (p >= index)
                                    {
                                        break;
                                    }
                                    k++;
                                }
                                newpath.Insert(k, index);
                                res = Math.Min(res, req(newpath) + Matrix[i, j]);
                                break;
                            }
                        }
                        
                    }
                }
                
            }

            cash.Add(hash, res);
            return res;
        }

        public static string GetHash(List<int> path)
        {
            var res = "!";
            foreach ( var i in path)
            {
                res += i.ToString() + " ";
            }
            return res;
        }
    }
}
