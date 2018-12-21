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
            var path = new List<int>();
            for(int i = 0; i < N; i++)
            {
                path.Add(i);
            }
            Console.WriteLine(req(path));
            Console.ReadLine();
            

        }

        static int req( List<int> path )
        {
            if (path.Count <= 3)
            {
                return 0;
            }

            var pathhash = GetHash(path);
            if ( cash.ContainsKey(pathhash)) 
            {
                return cash[pathhash];
            }

            var res = Int32.MaxValue;
            
            for (int i = 0; i < path.Count - 2; i++)
            {

                for (int j = i+2; j < path.Count && !(i==0 && j == path.Count-1); j++)
                {
                    var newpath1 = new List<int>();
                    var newpath2 = new List<int>();
                    int k = 0;
                    foreach(var vert in path)
                    {
                        if(k <= i || k >= j)
                        {
                            newpath1.Add(vert);
                        }
                        if( k >= i && k <= j)
                        {
                            newpath2.Add(vert);
                        }

                        k++;
                    }

                    var r1 = req(newpath1);
                    var r2 = req(newpath2);
                    res = Math.Min(res, r1 == int.MaxValue || r2 == int.MaxValue ? int.MaxValue : r1 + r2 + Matrix[path[i], path[j]]);

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
