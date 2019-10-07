using System;

namespace YarQual2019C
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = Console.ReadLine().Split(new char[] { ' ' });
            Console.WriteLine((Int32.Parse(s[2]) + Int32.Parse(s[3]) - 2) % 7 + 1);
            Console.ReadLine();
        }
    }
}
