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

    class Program
    {
        public static int[,] Matrix;
        public static int N;
        public static Dictionary<Int64, int> cash = new Dictionary<Int64, int>();
        public static SortedSet<Task> Pipe = new SortedSet<Task>();
        public static int BestScore = Int32.MaxValue;
        public static int Counter = 0;

        static int Req(Task task, int len)
        {
            if(Counter > 3300000)
            {
                return BestScore;
            }
            Counter++;

            if (task.Path.Length <= 3)
            {
                if(len < BestScore)
                {
                    BestScore = len;
                }
                return 0;
            }

            if ( cash.ContainsKey(task.Hash)) 
            {
                var cashv = cash[task.Hash];
                if (cashv + len < BestScore )
                {
                    BestScore = cashv + len;
                }
                return cashv;
            }

            var vertexes = new Task[task.Path.Length];
            for (int i = 0; i < task.Path.Length; i++)
            {
                var newpath = new int[task.Path.Length - 1];
                Array.Copy(task.Path, 0, newpath, 0, i);
                Array.Copy(task.Path, i + 1, newpath, i, task.Path.Length - i - 1);

                var v = task.Path[i];
                var v1 = task.Path[i == 0 ? task.Path.Length - 1 : i - 1];
                var v2 = task.Path[i == task.Path.Length - 1 ? 0 : i + 1];
                var cutValue = Matrix[v1, v2];
                var value = cutValue;
                for(int j = 0; j < task.Path.Length; j++)
                {
                    var curv = task.Path[j];
                    if (curv != v && curv != v1 && curv != v2)
                    {
                        value -= Matrix[v, curv];
                    }
                }
                vertexes[i] = new Task(value, newpath)
                {
                    CutValue = cutValue
                };
                
            }
            Array.Sort(vertexes);
            var res = Int32.MaxValue;
            foreach(var vertex in vertexes)
            {
                var r = Req(vertex, len + vertex.CutValue);
                res = Math.Min(res, r == int.MaxValue ? int.MaxValue : r + vertex.CutValue);
            }
            cash[task.Hash] = res;
            return res;
        }

        public static int Deep()
        {
            var path = new int[N];
            for (int i = 0; i < N; i++)
            {
                path[i] = i;
            }

            return Req(new Task(0, path), 0);
        }

        public static int Dijkstra()
        {
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

        public static Int64 GetHash(int[] path)
        {
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

            Deep();

            Console.WriteLine(BestScore);
            Console.ReadLine();
            

        }
    }
}
