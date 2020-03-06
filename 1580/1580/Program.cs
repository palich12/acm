using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1580
{

    public struct student
    {
        public bool isFormula;
        public bool isValue;

        public double freeCof;
        public int xCof;
        public double value;
    }


    class Program
    {
        static int EMPTY = -100000;
        static double sigma = 0.001;
        static int N, M;
        static int[,] Matrix;
        static student[] Students;



        static bool calcFormula(int index)
        {
            for (int i = 0; i < N; i ++)
            {
                if(Matrix[index,i] != EMPTY)
                {
                    if( !Students[i].isFormula)
                    {
                        Students[i].isFormula = true;
                        Students[i].freeCof = Matrix[index, i] - Students[index].freeCof;
                        Students[i].xCof = -1 * Students[index].xCof;

                        if (!calcFormula(i))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if ( Students[index].xCof == Students[i].xCof )
                        {
                            Students[i].isValue = true;
                            Students[i].value = ((double)Matrix[index,i] - Students[index].freeCof - Students[i].freeCof)*0.5;
                            return calcValue(i);
                        }
                        else if(  Math.Abs((double)Matrix[index, i] - Students[index].freeCof - Students[i].freeCof) > sigma  )
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        static bool calcValue(int index)
        {
            for (int i = 0; i < N; i++)
            {
                if (Matrix[index, i] != EMPTY)
                {
                    if (!Students[i].isValue)
                    {
                        Students[i].isValue = true;
                        Students[i].value = Matrix[index, i] - Students[index].value;

                        if (!calcValue(i))
                        {
                            return false;
                        }
                    }
                    else if (Math.Abs((double)Matrix[index, i] - Students[index].value - Students[i].value) > sigma)
                    { 
                        return false;
                    }
                }
            }

            return true;
        }

        static void Main(string[] args)
        {
            var NM = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
            N = NM[0];
            M = NM[1];
            Matrix = new int[N, N];
            Students = new student[N];
            for(int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Matrix[i, j] = EMPTY;
                }
            }

            for ( int i = 0; i < M; i++)
            {
                var line = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
                Matrix[line[0]-1, line[1]-1] = line[2];
            }

            for (int i = 0; i < N; i++)
            {
                if (!Students[i].isValue)
                {
                    if ( !Students[i].isFormula ) {

                        Students[i].isFormula = true;
                        Students[i].xCof = 1;
                        Students[i].freeCof = 0;
                        
                        if (!calcFormula(i))
                        {
                            Console.WriteLine("IMPOSSIBLE");
                            Console.ReadLine();
                            return;
                        }

                    }
                    else
                    {
                        Console.WriteLine("IMPOSSIBLE");
                        Console.ReadLine();
                        return;
                    }
                }
            }

            for(int i = 0; i < N; i++)
            {
                Console.WriteLine(Students[i].value.ToString("F", CultureInfo.InvariantCulture));
            }

            Console.ReadLine();
        }
    }
}
