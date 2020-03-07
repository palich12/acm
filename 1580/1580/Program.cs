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
        public Int64 freeCof;
        public Int64 xCof;
        public double value;
    }


    class SuccessException : Exception
    {

    }

    class ErrorException : Exception
    {

    }

    class Program
    {
        static int EMPTY = -100000;
        static double sigma = 0.00000001;
        static int N, M;
        static int[,] Matrix;
        static student[] Students;

        static void calcFormula(int index)
        {
            for (int i = 0; i < N; i++)
            {
                if (Matrix[index, i] != EMPTY)
                {
                    if (!Students[i].isFormula)
                    {
                        Students[i].isFormula = true;
                        Students[i].freeCof = Matrix[index, i] - Students[index].freeCof;
                        Students[i].xCof = -1 * Students[index].xCof;

                        calcFormula(i);
                    }
                    else
                    {
                        if (Students[index].xCof == Students[i].xCof)
                        {
                            Students[i].isValue = true;
                            Students[i].value = ((double)((Int64)Matrix[index, i] - Students[index].freeCof - Students[i].freeCof)) * 0.5;
                            calcValue(i);
                            throw new SuccessException();
                        }
                        else if (Math.Abs((double)Matrix[index, i] - Students[index].freeCof - Students[i].freeCof) > sigma)
                        {
                            throw new ErrorException();
                        }
                    }
                }
            }
        }

        static void calcValue(int index)
        {
            for (int i = 0; i < N; i++)
            {
                if (Matrix[index, i] != EMPTY)
                {
                    if (!Students[i].isValue)
                    {
                        Students[i].isValue = true;
                        Students[i].value = Matrix[index, i] - Students[index].value;

                        calcValue(i);
                    }
                    else if (Math.Abs((double)Matrix[index, i] - Students[index].value - Students[i].value) > sigma)
                    {
                        throw new ErrorException();
                    }
                }
            }
        }

        static void cleanSegment(int index)
        {

            Students[index].isFormula = false;
            Students[index].isValue = false;
            for (int i = 0; i < N; i++)
            {
                if (Matrix[index, i] != EMPTY && (Students[i].isValue || Students[i].isFormula))
                {
                    cleanSegment(i);
                }
            }
        }

        static void Main(string[] args)
        {
            var NM = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
            N = NM[0];
            M = NM[1];
            Matrix = new int[N, N];
            Students = new student[N];
            
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Matrix[i, j] = EMPTY;
                }
            }

            for (int i = 0; i < M; i++)
            {
                var line = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
                Matrix[line[0] - 1, line[1] - 1] = line[2];
                Matrix[line[1] - 1, line[0] - 1] = line[2];
            }

            for (int i = 0; i < N; i++)
            {
                if (!Students[i].isValue)
                {
                    Students[i].isFormula = true;
                    Students[i].xCof = 1;
                    Students[i].freeCof = 0;

                    try
                    {
                        calcFormula(i);
                        throw new ErrorException();
                    }
                    catch( SuccessException)
                    {

                    }
                    catch (ErrorException)
                    {
                        cleanSegment(i);
                    }
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (!Students[i].isValue)
                {
                    Console.WriteLine("IMPOSSIBLE");
                    Console.ReadLine();
                    return;
                }
            }

            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(Students[i].value.ToString("F", CultureInfo.InvariantCulture));
            }

            Console.ReadLine();
        }
    }
}
