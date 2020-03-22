using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectVersion
{
    class SegmentComparer : IComparer<Hobbit[]>
    {
        public int Compare(Hobbit[] x, Hobbit[] y)
        {
            return x.Length.CompareTo(y.Length);
        }
    }

    class Hobbit
    {
        public static List<Hobbit> All = new List<Hobbit>();
        public static Hobbit[][] Segments;

        public int Id = 0;
        public int SegmentId = 0;
        public List<Hobbit> Lighter = new List<Hobbit>();
        public List<Hobbit> Heavier = new List<Hobbit>();
        public List<Hobbit> Friendship = new List<Hobbit>();

        public Hobbit(int id)
        {
            Id = id;
            All.Add(this);
        }

        public void remove()
        {
            foreach ( var hobbit in Lighter )
            {
                hobbit.Heavier.Remove(this);
                hobbit.Heavier.AddRange(Heavier);
            }
            foreach (var hobbit in Heavier)
            {
                hobbit.Lighter.Remove(this);
                hobbit.Lighter.AddRange(Lighter);
            }

            All.Remove(this);
        }
    }


    class Program
    {

        static DateTime Timer;

        static int N;

        static void fillMatrix( Hobbit hobbit, List<Hobbit> lighter)
        {
            foreach ( var l in lighter )
            {
                if( !hobbit.Lighter.Contains(l))
                {
                    hobbit.Lighter.Add(l);
                    l.Heavier.Add(hobbit);
                }
            }

            lighter.Add(hobbit);

            var heavier = hobbit.Heavier.ToArray();
            foreach ( var h in heavier)
            {
                fillMatrix(h, lighter);
            }
            lighter.Remove(hobbit);
        }

        static void fillSegment( int segmentId, Hobbit hobbit, List<Hobbit> segment )
        {
            if( hobbit.SegmentId != 0)
            {
                return;
            }
            hobbit.SegmentId = segmentId;
            segment.Add(hobbit);

            foreach( var h in hobbit.Heavier)
            {
                fillSegment(segmentId, h, segment);
            }
            foreach (var h in hobbit.Lighter)
            {
                fillSegment(segmentId, h, segment);
            }
        }

        static Hobbit[] Result = null;
        static void BronKerbosch1(Hobbit[] R, Hobbit[] P, Hobbit[] X)
        {
            if( P.Length == 0 && X.Length == 0)
            {
                if (Result == null || Result.Length < R.Length)
                {
                    Result = R;
                }
                return;
            }

            
            if ( (DateTime.Now - Timer).TotalMilliseconds > 100 )
            {
                throw new Exception();
            }

            var newP = P.ToList();
            var newX = X.ToList();
            foreach( var v in P)
            {
                BronKerbosch1(
                    R.Concat(new Hobbit[] { v }).ToArray(),
                    newP.Where(p => v.Friendship.Contains(p)).ToArray(),
                    newX.Where(p => v.Friendship.Contains(p)).ToArray());

                newP.Remove(v);
                if( !newX.Contains(v))
                {
                    newX.Add(v);
                }
            }
        }

        static void Main(string[] args)
        {

            //read data
            N = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < N; i++)
            {
                new Hobbit(i);
            }


            for (int i = 0; i < N; i++)
            {
                var line = Console.ReadLine().Split().Select(x => Int32.Parse(x)).ToArray();
                for (int j = 0; j < N; j ++)
                {
                    if (line[j] == 1)
                    {
                        Hobbit.All[i].Lighter.Add(Hobbit.All[j]);
                        Hobbit.All[j].Heavier.Add(Hobbit.All[i]);
                    }
                }
            }
            Timer = DateTime.Now;

            //remove chains
            //bool end = true;
            //do
            //{
            //    end = true;
            //    foreach ( var hobbit in Hobbit.All)
            //    {
            //        if (hobbit.Lighter.Count == 1 && hobbit.Lighter[0].Heavier.Count == 1)
            //        {
            //            hobbit.remove();
            //            end = false;
            //            break;
            //        }
            //    }

            //} while ( !end );

            //fill matrix and detect segments
            int segmentId = 1;
            var segments = new List<Hobbit[]>();
            foreach( var hobbit in Hobbit.All)
            {
                if (hobbit.Lighter.Count == 0)
                {
                    fillMatrix( hobbit, new List<Hobbit>() );
                }

                if( hobbit.SegmentId == 0)
                {
                    var newSegment = new List<Hobbit>();
                    fillSegment(segmentId, hobbit, newSegment);
                    segments.Add(newSegment.ToArray());
                    segmentId++;
                }
            }
            Hobbit.Segments = segments.ToArray();
            Array.Sort(Hobbit.Segments, new SegmentComparer());

            //create final matrix
            foreach ( var segment in Hobbit.Segments )
            {
                for(int i = 0; i < segment.Length; i++)
                {
                    for (int j = i+1; j < segment.Length; j++)
                    {
                        if (!segment[i].Heavier.Contains(segment[j]) && !segment[i].Lighter.Contains(segment[j]))
                        {
                            segment[i].Friendship.Add(segment[j]);
                            segment[j].Friendship.Add(segment[i]);
                        }
                    }
                }
            }

            var result = new List<Hobbit>();
            foreach (var segment in Hobbit.Segments)
            {
                Result = null;
                try
                {
                    BronKerbosch1(new Hobbit[] { }, segment.ToArray(), new Hobbit[] { });
                }
                catch (Exception) { }
                
                if(Result == null)
                {
                    Result = new Hobbit[] {segment[0]};
                }
                result.AddRange(Result);
            }

            //output
            Console.WriteLine(result.Count);
            Console.WriteLine(string.Join(" ", result.Select(x => (x.Id + 1).ToString()).ToArray()));

            Console.ReadLine();
        }
    }
}
