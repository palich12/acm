using System;
using System.Collections;
using System.Collections.Generic;

namespace TulaI
{
    class Task : IComparable
    {
        public static int IndexCounter = 0;
        public int Index;
        public int Value;
        public int[] Path;
        public Int64 Hash;
        public int CutValue;

        public Task(int value, int[] path)
        {
            Path = path;
            Value = value;
            Hash = Program.GetHash(Path);
            Index = IndexCounter;
            IndexCounter++;
        }

        public int CompareTo(object obj)
        {
            var r = Value.CompareTo((obj as Task).Value);
            if(r == 0)
            {
                return Index.CompareTo((obj as Task).Index);
            }
            return r;
        }
    }

    class Task2 : Task
    {
        public List<int[]> Parts;
        public Task2(int value, List<int[]> parts) : base(value, null)
        {
            Parts = parts;
            Hash = GetHash2(Parts);
        }

        public static Int64 GetHash2(List<int[]> parts)
        {
            Int64 res = 1;
            foreach (var part in parts)
            {
                for (var i = 0; i < part.Length; i++)
                {
                    //res *= i;
                    res *= 500;
                    res += part[i];
                }
                res *= 500;
                res += Program.N;
            }
            
            return res;
        }
    }

    class ReqResult
    {
        public int Value = 0;
        public bool AllowCash = true;
    }

    class Program
    {
        public static int[,] Matrix;
        public static int N;
        public static Dictionary<Int64, int> cash = new Dictionary<Int64, int>();
        public static int BestScore = Int32.MaxValue;
        public static int Counter = 0;
        //public static int CounterMax = 4300000;
        public static int CounterMax = 3300000;

        static ReqResult Req(Task task, int len)
        {
            Counter++;

            if (Counter > CounterMax || len >= BestScore)
            {
                return new ReqResult() {
                    Value = BestScore,
                    AllowCash = false
                };
            }
            

            if (task.Path.Length <= 3)
            {
                if(len < BestScore)
                {
                    BestScore = len;
                }
                return new ReqResult();
            }

            if ( cash.ContainsKey(task.Hash)) 
            {
                var cashv = cash[task.Hash];
                if (cashv + len < BestScore )
                {
                    BestScore = cashv + len;
                }
                return new ReqResult()
                {
                    Value = cashv
                };
            }

            var vertexes = new SortedSet<Task>();
            var isSkiped = false;
            for (int i = 0; i < task.Path.Length; i++)
            {
                var vert = task.Path[i];
                var prev = task.Path[i == 0 ? task.Path.Length - 1 : i - 1];
                var next = task.Path[i == task.Path.Length - 1 ? 0 : i + 1];
                var cutValue = Matrix[prev, next];
                if(len+cutValue >= BestScore)
                {
                    isSkiped = true;
                    continue;
                }

                var newpath = new int[task.Path.Length - 1];
                Array.Copy(task.Path, 0, newpath, 0, i);
                Array.Copy(task.Path, i + 1, newpath, i, task.Path.Length - i - 1);
             
                var value = cutValue;
                for(int j = 0; j < task.Path.Length; j++)
                {
                    var curv = task.Path[j];
                    if (curv != vert && curv != prev && curv != next)
                    {
                        value -= Matrix[vert, curv];
                    }

                    if (Counter > CounterMax)
                    {
                        return new ReqResult()
                        {
                            Value = BestScore,
                            AllowCash = false
                        };
                    }
                }

                vertexes.Add(new Task(value, newpath)
                {
                    CutValue = cutValue
                });

            }
            
            var result = Int32.MaxValue;
            var allowCash = false;
            foreach(var vertex in vertexes)
            {
                var r = Req(vertex, len + vertex.CutValue);
                var rv = r.Value == int.MaxValue ? int.MaxValue : r.Value + vertex.CutValue;
                if( result > rv)
                {
                    result = rv;
                    allowCash = r.AllowCash;
                }
                else if ( result == rv && r.AllowCash)
                {
                    allowCash = r.AllowCash;
                }

                if (Counter > CounterMax)
                {
                    return new ReqResult()
                    {
                        Value = BestScore,
                        AllowCash = false
                    };
                }
            }

            allowCash = allowCash && !isSkiped;

            if (allowCash)
            {
                cash[task.Hash] = result;
            }
            
            return new ReqResult() {
                Value = result,
                AllowCash = allowCash
            };
        }

        public static int Deep()
        {
            var path = new int[N];
            for (int i = 0; i < N; i++)
            {
                path[i] = i;
            }

            return Req(new Task(0, path), 0).Value;
        }

        public static int Dijkstra()
        {
            var Pipe = new SortedSet<Task>();

            var path = new int[N];
            for (int i = 0; i < N; i++)
            {
                path[i] = i;
            }

            Pipe.Add(new Task(0, path));

            while (Pipe.Count > 0)
            {
                Task task = (Task)Pipe.Min;
                Pipe.Remove(task);

                if (task.Path.Length <= 3)
                {
                    BestScore = task.Value;
                    return task.Value;
                }

                if (cash.ContainsKey(task.Hash))
                {
                    continue;
                }
                cash[task.Hash] = task.Value;

                for (int i = 0; i < task.Path.Length; i++)
                {
                    var newpath = new int[task.Path.Length - 1];
                    Array.Copy(task.Path, 0, newpath, 0, i);
                    Array.Copy(task.Path, i + 1, newpath, i, task.Path.Length - i - 1);
                    var value = task.Value + Matrix[task.Path[i == 0 ? task.Path.Length - 1 : i - 1], task.Path[i == task.Path.Length - 1 ? 0 : i + 1]];
                    var newtask = new Task(value, newpath);
                    if (cash.ContainsKey(newtask.Hash))
                    {
                        continue;
                    }
                    Pipe.Add(newtask);
                }

            }

            return BestScore;
        }

        public static int Dijkstra2()
        {
            var Pipe = new SortedSet<Task2>();

            var path = new int[N];
            for (int i = 0; i < N; i++)
            {
                path[i] = i;
            }

            Pipe.Add(new Task2(0, new List<int[]> { path }));

            while (Pipe.Count > 0)
            {
                var task = Pipe.Min;
                Pipe.Remove(task);

                if (task.Parts.Count == 0)
                {
                    BestScore = task.Value;
                    return task.Value;
                }

                if (cash.ContainsKey(task.Hash))
                {
                    continue;
                }
                cash[task.Hash] = task.Value;


                for (int i = 0; i < task.Parts.Count; i ++)
                {
                    path = task.Parts[i];

                    for (int j = 0; j < path.Length - 2; j++)
                    {
                        for (int k = j + 2; k < path.Length; k++ )
                        {
                            var newparts = new List<int[]>();
                            for( var i1 = 0; i1 < task.Parts.Count; i1++)
                            {
                                if ( i1 == i )
                                {
                                    if ((j + 1) + (path.Length - k) > 3)
                                    {
                                        var newpath = new int[(j + 1) + (path.Length - k)];
                                        Array.Copy(path, 0, newpath, 0, j + 1);
                                        Array.Copy(path, k, newpath, j + 1, path.Length - k);
                                        newparts.Add(newpath);
                                    }
                                    if (k - j + 1 > 3 )
                                    {
                                        var newpath = new int[k - j + 1];
                                        Array.Copy(path, j, newpath, 0, newpath.Length);
                                        newparts.Add(newpath);
                                    }
                                }
                                else
                                {
                                    newparts.Add(task.Parts[i1]);
                                }
                            }

                            var value = task.Value + Matrix[path[j], path[k]];
                            var newtask = new Task2(value, newparts);
                            if (cash.ContainsKey(newtask.Hash))
                            {
                                continue;
                            }
                            Pipe.Add(newtask);
                        }

                        
                    }
                }
            }

            return BestScore;
        }

        public static Int64 GetHash(int[] path)
        {
            if (path == null)
            {
                return 0;
            }
            Int64 res = 1;
            for ( var i = 0; i < path.Length; i++)
            {
                //res *= i;
                res *= 500;
                res += path[i];
            }
            return res;  
        }

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

            //Deep();
            Dijkstra2();

            Console.WriteLine(BestScore);
            Console.ReadLine();
            

        }
    }
}
