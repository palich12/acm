using System;
using System.Collections;
using System.Collections.Generic;

namespace TulaI
{

    public class Entity : IComparable
    {
        public List<int> path;
        public int w;

        public int CompareTo(object obj)
        {
            return w.CompareTo(((Entity)obj).w);
        }
    }

    public class ReqResult
    {
        public int Result;
        public bool Uncash;
    }
    class Program
    {
        public static int[,] Matrix;
        public static int N;
        public static Dictionary<Int64, int> cash = new Dictionary<Int64, int>();
        public static int BestScore = Int32.MaxValue;
        public static int Counter = 0;

        static void Main(string[] args)
        {
            N = Int32.Parse(Console.ReadLine());
            Matrix = new int[N,N];

            for(int i = 0; i < N; i++)
            {
                var l = Console.ReadLine().Split(new char[] { ' ' });
                for(int j=0; j < N; j++)
                {
                    Matrix[i, j] = Int32.Parse(l[j]);
                }
            }

            //req(new List<int>());
            
            Console.WriteLine(req(new List<int>()));
            //Console.ReadLine();
            

        }

        static int req( List<int> path )
        {
            if (N - path.Count <= 3)
            {
                return 0;
            }

            var pathhash = GetHash(path);
            if ( cash.ContainsKey(pathhash)) 
            {
                return cash[pathhash];
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
                                var newpath = new List<int>(path)
                                    {
                                        index
                                    };
                                newpath.Sort();

                                var r = req(newpath);
                                res = Math.Min(res, r == int.MaxValue ? int.MaxValue : r + Matrix[i, j]);
                                break;
                            }
                        }
                        
                    }
                }              
            }

            cash[pathhash] = res;
            return res;
        }

        public static Int64 GetHash(List<int> path)
        {
            Int64 res = 1;
            for ( var i = 0; i < path.Count; i++)
            {
                //res *= i;
                res *= 500;
                res += path[i];
            }
            return res;  
        }
    }
}
