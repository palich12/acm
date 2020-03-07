using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1580
{
    public class Student
    {
        public int index = 0;
        public bool isFormula = false;
        public bool isValue = false;
        public double freeCof = 0;
        public double xCof = 0;
        public double value = 0;
    }

    public class Pair 
    {
        public int index = 0;
        public double value = 0;
    }


    class SuccessException : Exception
    {

    }

    class ErrorException : Exception
    {

    }

    class Program
    {
        static double sigma = 0.00000001;
        static int N, M;
        static Pair[][] Matrix;
        static Student[] Students;
        static double? X = null;

        static void calcFormula(Student student)
        {
            foreach ( var pair in Matrix[student.index] )
            {
                var nextStudent = Students[pair.index];
                if (!nextStudent.isFormula)
                {
                    nextStudent.isFormula = true;
                    nextStudent.freeCof = pair.value - student.freeCof;
                    nextStudent.xCof = -1.0 * student.xCof;

                    calcFormula(nextStudent);
                }
                else
                {
                    var k = student.xCof + nextStudent.xCof;
                    var b = pair.value - student.freeCof - nextStudent.freeCof;
                    
                    if (k != 0)
                    {
                        X = b / k;
                    }
                    else if (Math.Abs(b) > sigma)
                    {
                        throw new ErrorException();
                    }
                }
            }
        }

        static void calcValue(Student student)
        {
            foreach (var pair in Matrix[student.index])
            {
                var nextStudent = Students[pair.index];
                if (!nextStudent.isValue)
                {
                    nextStudent.isValue = true;
                    nextStudent.value = pair.value - student.value;

                    calcValue(nextStudent);
                }
            }
        }

        static void Main(string[] args)
        {
            var NM = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
            N = NM[0];
            M = NM[1];
            Matrix = new Pair[N][];
            Students = new Student[N];

            var preMatrix = new List<Pair>[N];
            for(int i = 0; i < N; i++)
            {
                preMatrix[i] = new List<Pair>();
            }

            for (int i = 0; i < M; i++)
            {
                var line = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
                preMatrix[line[0] - 1].Add(new Pair() {
                    index = line[1] - 1,
                    value = line[2]
                });
                preMatrix[line[1] - 1].Add(new Pair()
                {
                    index = line[0] - 1,
                    value = line[2]
                });
            }

            for (int i = 0; i < N; i++)
            {
                Matrix[i] = preMatrix[i].ToArray();
                Students[i] = new Student() {
                    index = i
                };
            }

            foreach (var student in Students)
            {
                if (!student.isFormula)
                {
                    student.isFormula = true;
                    student.xCof = 1;
                    student.freeCof = 0;

                    try
                    {
                        X = null;
                        calcFormula(student);
                        if(X is null)
                        {
                            throw new ErrorException();
                        }
                        else
                        {
                            student.isValue = true;
                            student.value = X ?? 0;
                            calcValue(student);
                        }
                    }
                    catch(ErrorException)
                    {
                        Console.WriteLine("IMPOSSIBLE");
                        Console.ReadLine();
                        return;
                    }

                }
            }

            foreach (var student in Students)
            {
                Console.WriteLine(student.value.ToString("F", CultureInfo.InvariantCulture));
            }

            Console.ReadLine();
        }
    }
}
