using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1533
{
    class Program
    {
        static int N;
        static int[][] Matrix;
        static int[] Components;
        static int ComponentsCount = 0;

        static void fillMatrix( int hobbit, List<int> lights )
        {

            foreach( var light in lights)
            {
                Matrix[hobbit][light] = 1;
                Matrix[light][hobbit] = -1;
            }

            for (int i = 0; i < N; i++)
            {
                if(Matrix[hobbit][i] == -1)
                {
                    lights.Add(hobbit);
                    fillMatrix(i, lights);
                    lights.Remove(hobbit);
                }
            }

        }

        static void fillComponent( int index, int hobbit)
        {
            if( Components[hobbit] != 0)
            {
                return;
            }
            Components[hobbit] = index;

            for ( int i = 0; i < N; i++ )
            {
                if(Matrix[hobbit][i] != 0)
                {
                    fillComponent(index, i);
                }
            }
        }

        static void Main(string[] args)
        {
            N = Int32.Parse(Console.ReadLine());
            Matrix = new int[N][];
            Components = new int[N];
            for( int i = 0; i < N; i++)
            {
                Matrix[i] = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
            }


            //fill -1 matrix
            for(int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if(Matrix[i][j] == 1)
                    {
                        Matrix[j][i] = -1;
                    }
                }
            }

            //fill full 1 matrix
            for (int i = 0; i < N; i++)
            {
                bool noLight = true;
                for (int j = 0; j < N; j++)
                {
                    if (Matrix[i][j] == 1)
                    {
                        noLight = false;
                    }
                }

                if (noLight)
                {
                    fillMatrix(i, new List<int>());
                }
            }

            //fill componets
            for(int i = 0; i < N; i++)
            {
                if ( Components[i] == 0 )
                {
                    ComponentsCount++;
                    fillComponent(ComponentsCount, i);
                }
            }

            //find max zeros in components
            var result = new List<int>();
            for ( int compId = 1; compId <= ComponentsCount; compId++ )
            {
                int max = 0;
                int maxid = -1;
                for (int i = 0; i < N; i++)
                {

                    if(Components[i] != compId)
                    {
                        continue;
                    }

                    int count = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (Components[j] != compId)
                        {
                            continue;
                        }

                        if (Matrix[i][j] == 0)
                        {
                            count++;
                        }

                        if (count > max)
                        {
                            max = count;
                            maxid = i;
                        }
                    }
                }

                //add max zeros to result 
                for (int i = 0; i < N; i++)
                {
                    if (Matrix[maxid][i] == 0 && Components[i] == compId)
                    {
                        result.Add(i);
                    }
                }
            }

            //write result
            Console.WriteLine(result.Count);
            Console.WriteLine(string.Join(" ", result.Select(x => (x+1).ToString()).ToArray()));

            //Console.ReadLine();
        }
    }
}
