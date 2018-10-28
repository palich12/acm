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
        public static Dictionary<string, int> cash = new Dictionary<string, int>();
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

            req(new List<int>(), 0);
            Console.WriteLine(BestScore);
            //Console.ReadLine();
            

        }

        static ReqResult req( List<int> path, int curscore )
        {
            bool uncash = false;
            Counter++;
            if (Counter > 200000)
            {
                return new ReqResult()
                {
                    Result = Int32.MaxValue,
                    Uncash = true
                };
            }

            if(N - path.Count <= 3)
            {
                if(BestScore > curscore)
                {
                    BestScore = curscore;
                }
                cash.Add(GetHash(path), 0);
                return new ReqResult()
                {
                    Result = 0,
                    Uncash = false
                };
            }

            var pathhash = GetHash(path);
            if ( cash.ContainsKey(pathhash)) 
            {
                return new ReqResult()
                {
                    Result = cash[pathhash],
                    Uncash = false
                };
            }

            
            var entities = new List<Entity>();
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

                                var newpath = new List<int>(path);
                                newpath.Add(index);
                                newpath.Sort();
                                if (curscore + Matrix[i, j] < BestScore)
                                {
                                    entities.Add(new Entity() {
                                        path = newpath,
                                        w = Matrix[i, j],
                                    });
                                }
                                else
                                {
                                    uncash = true;
                                }
                                break;
                            }
                        }
                        
                    }
                }              
            }

            entities.Sort();
            var res = Int32.MaxValue;
            foreach(var e in entities)
            {
                var r = req(e.path, curscore + e.w);
                res = Math.Min(res, r.Result);
                uncash = uncash || r.Uncash;
            }

            if (!uncash)
            {
                cash.Add(pathhash, res);
            }

            return new ReqResult()
            {
                Result = res,
                Uncash = uncash
            };
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
