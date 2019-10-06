using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivan
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 4, y = 8;

            var matrix = new ushort[x, y];


            matrix[0, 0] = 0b1000000000000000;
            for(int i=0; i < x; i++)
            {
                var res = "";
                for (int j=0; j < y; j++)
                {

                    ushort left = i - 1 < 0 ? (ushort)0 : (ushort)(matrix[i - 1, j] /2);
                    ushort down = j - 1 < 0 ? (ushort)0 : (ushort)(matrix[i, j - 1] /2);
                    if(i != 0 || j != 0)
                        matrix[i, j] = (ushort)(left + down);

                    var r = Convert.ToString(matrix[i, j], 2);
                    while (r.Length < 16)
                    {
                        r = "0" + r;
                    }
                    res += " " + r;
                }
                Console.WriteLine(res);
            }
            
            Console.Read();
        }
    }
}
