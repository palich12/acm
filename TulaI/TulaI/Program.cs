﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace TulaI
{
    class Task : IComparable
    {
        public static int Indexer = 0;
        public int Index;
        public int Value;
        public int[] Path;

        public Task()
        {
            Index = Indexer;
            Indexer++;
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

        static int Req( int[] path)
        {
            if (path.Length <= 3)
            {
                return 0;
            }

            var pathhash = GetHash(path);
            if ( cash.ContainsKey(pathhash)) 
            {
                return cash[pathhash];
            }  
            var res = Int32.MaxValue;
            for ( int i = 0; i < path.Length; i ++)
            {
                var newpath = new int[path.Length - 1];
                Array.Copy(path, 0, newpath, 0, i);
                Array.Copy(path, i + 1, newpath, i, path.Length - i - 1);
                var r = Req(newpath);
                res = Math.Min(res, r == int.MaxValue ? int.MaxValue : r + Matrix[path[i == 0 ? path.Length - 1 : i - 1], path[i == path.Length - 1 ? 0 : i + 1]]);
            }
            cash[pathhash] = res;
            return res;
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

            //req(new List<int>());
            var path = new int[N];
            for(int i = 0; i < N; i++)
            {
                path[i] = i;
            }

            Pipe.Add( new Task
            {
                Path = path,
                Value = 0
            });

            while(Pipe.Count > 0)
            {
                Task task = (Task)Pipe.Min;
                Pipe.Remove(task);

                if ( task.Path.Length <= 3)
                {
                    BestScore = task.Value;
                    break;
                }
                for (int i = 0; i < task.Path.Length; i++)
                {
                    var newpath = new int[task.Path.Length - 1];
                    Array.Copy(task.Path, 0, newpath, 0, i);
                    Array.Copy(task.Path, i + 1, newpath, i, task.Path.Length - i - 1);
                    var value = task.Value + Matrix[task.Path[i == 0 ? task.Path.Length - 1 : i - 1], task.Path[i == task.Path.Length - 1 ? 0 : i + 1]];
                    Pipe.Add( new Task()
                    {
                        Value = value,
                        Path = newpath
                    });                                    
                }

            }

            Console.WriteLine(BestScore);
            Console.ReadLine();
            

        }
    }
}
