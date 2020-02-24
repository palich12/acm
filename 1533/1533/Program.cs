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
        static int[] ComponentSizes;
        static int ComponentsCount = 0;
        static int callCount = 0;
        static int callCountLimit = 4500000;

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
            ComponentSizes[index]++;

            for ( int i = 0; i < N; i++ )
            {
                if(Matrix[hobbit][i] != 0)
                {
                    fillComponent(index, i);
                }
            }
        }

        static bool checkClique( int[] hobbits )
        {
            for ( int i = 0; i < hobbits.Length - 1; i ++ )
            {
                for ( int j = i + 1; j < hobbits.Length; j++ )
                {
                    if ( Matrix[hobbits[i]][hobbits[j]] > 0 )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static int[] checkAllCliqueOfSize( int component, int start, int size, int heapSize, List<int> prevList)
        {

            if (callCount > callCountLimit)
            {
                return null;
            }
            callCount++;


            if (size == 0)
            {
                var result = prevList.ToArray();
                if( checkClique(result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }

            if ( start >= N || heapSize == 0)
            {
                return null;
            }

            int newHeapSize = heapSize;
            for ( int i = start; i < N && newHeapSize >= size; i++ )
            {

                if ( Components[i] == component )
                {
                    newHeapSize--;
                    var newPrewList = prevList.ToList();
                    newPrewList.Add(i);
                    var result = checkAllCliqueOfSize(component, i + 1, size - 1, newHeapSize, newPrewList);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        static void Main(string[] args)
        {
            N = Int32.Parse(Console.ReadLine());
            Matrix = new int[N][];
            Components = new int[N];
            ComponentSizes = new int[N+1];
            for ( int i = 0; i < N; i++)
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

            //find max clique in components
            var results = new int[ComponentsCount][];
            var resultsComplite = new bool[ComponentsCount];

            int I = 1;
            do
            {
                for (int compId = 1; compId <= ComponentsCount; compId++)
                {
                    if (!resultsComplite[compId - 1] && I <= ComponentSizes[compId] - I + 1)
                    {
                        var newClique = checkAllCliqueOfSize(compId, 0, I, ComponentSizes[compId], new List<int>());
                        if (newClique != null)
                        {
                            results[compId - 1] = newClique;

                        }

                        if (ComponentSizes[compId] - I + 1 > I)
                        {
                            newClique = checkAllCliqueOfSize(compId, 0, ComponentSizes[compId] - I + 1, ComponentSizes[compId], new List<int>());
                            if (newClique != null)
                            {
                                results[compId - 1] = newClique;
                                resultsComplite[compId - 1] = true;
                            }
                        }
                        else
                        {
                            resultsComplite[compId - 1] = true;
                        }
                        
                    }
                    else
                    {
                        resultsComplite[compId - 1] = true;
                    }

                }

                I++;
            } while ( resultsComplite.Any( rc => !rc) && callCount < callCountLimit);

            //write result
            var result = new List<int>();
            foreach (var r in  results)
            {
                result.AddRange(r);
            }
            Console.WriteLine(result.Count);
            Console.WriteLine(string.Join(" ", result.Select(x => (x+1).ToString()).ToArray()));

            Console.ReadLine();
        }
    }
}
