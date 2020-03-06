using System;
using System.Collections.Generic;
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
        static int N, M;
        static int[,] Matrix;
        static student[] Students;



        static bool calcFormula(int index)
        {
            for (int i = 0; i < N; i ++)
            {

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
            for ( int i = 0; i < M; i++)
            {
                var line = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
                Matrix[line[0], line[1]] = line[2];
            }

            for (int i = 0; i < N; i++)
            {
                if (!Students[i].isValue)
                {

                }
            }
        }
    }
}
