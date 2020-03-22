using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectVersion
{
    class Hobbit
    {
        public static List<Hobbit> All = new List<Hobbit>();
        public static List<List<Hobbit>> Segments = new List<List<Hobbit>>();
        public static bool[,] friendshipMatrix;

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

        static List<Hobbit> findClick( int size, List<Hobbit> segment )
        {
            var candidates = new List<Hobbit>();
            foreach( var hobbit in segment)
            {
                if ( hobbit.Friendship.Count >= size - 1)
                {
                    candidates.Add(hobbit);
                }
            }

            if(candidates.Count < size)
            {
                return null;
            }

            return generareClick(new List<Hobbit>(), candidates, size, 0);
        }

        static List<Hobbit> generareClick( List<Hobbit> preClick, List<Hobbit> candidates, int freeCount, int startIndex)
        {
            if( freeCount == 0)
            {
                //check click
                for ( int i = 0; i < preClick.Count - 1; i++ )
                {
                    for (int j = i+1; j < preClick.Count; j++)
                    {
                        if ( !Hobbit.friendshipMatrix[preClick[i].Id, preClick[j].Id] )
                        {
                            return null;
                        }
                    }
                }

                return preClick;
            }

            //add next candidate
            for( int i = startIndex; i <= candidates.Count - freeCount; i ++)
            {
                preClick.Add(candidates[i]);
                var result = generareClick(preClick, candidates, freeCount-1, i+1);
                if (result != null)
                {
                    return result;
                }
                preClick.Remove(candidates[i]);
            }

            return null;
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
            Hobbit.friendshipMatrix = new bool[N, N];
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

            //remove chains
            bool end = true;
            do
            {
                end = true;
                foreach ( var hobbit in Hobbit.All)
                {
                    if (hobbit.Lighter.Count == 1 && hobbit.Lighter[0].Heavier.Count == 1)
                    {
                        hobbit.remove();
                        end = false;
                        break;
                    }
                }

            } while ( !end );
            
            //fill matrix and detect segments
            int segmentId = 1;
            foreach( var hobbit in Hobbit.All)
            {
                if (hobbit.Lighter.Count == 0)
                {
                    fillMatrix( hobbit, new List<Hobbit>() );
                }

                if( hobbit.SegmentId == 0)
                {
                    var newSegment = new List<Hobbit>();
                    Hobbit.Segments.Add(newSegment);
                    fillSegment(segmentId, hobbit, newSegment);
                    segmentId++;
                }
            }

            //create final matrix
            foreach ( var segment in Hobbit.Segments )
            {
                for(int i = 0; i < segment.Count; i++)
                {
                    for (int j = i+1; j < segment.Count; j++)
                    {
                        if (!segment[i].Heavier.Contains(segment[j]) && !segment[i].Lighter.Contains(segment[j]))
                        {
                            segment[i].Friendship.Add(segment[j]);
                            segment[j].Friendship.Add(segment[i]);
                            Hobbit.friendshipMatrix[segment[i].Id, segment[j].Id] = true;
                            Hobbit.friendshipMatrix[segment[j].Id, segment[i].Id] = true;
                        }
                    }
                }
            }

            //find click
            //var result = new List<Hobbit>();
            //foreach (var segment in Hobbit.Segments)
            //{
            //    List<Hobbit> click = null;
            //    for (int i = segment.Count; i > 0; i--)
            //    {
            //        click = findClick(i, segment);
            //        if (click != null)
            //        {
            //            result.AddRange(click);
            //            break;
            //        }
            //    }
            //}

            var result = new List<Hobbit>();
            foreach (var segment in Hobbit.Segments)
            {
                Result = null;
                BronKerbosch1(new Hobbit[] { }, segment.ToArray(), new Hobbit[] { });
                result.AddRange(Result);
            }

            //output
            Console.WriteLine(result.Count);
            Console.WriteLine(string.Join(" ", result.Select(x => (x.Id + 1).ToString()).ToArray()));

            Console.ReadLine();
        }
    }
}
