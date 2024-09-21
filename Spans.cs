using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayPoolExtensions
{
    [MemoryDiagnoser]
    [RankColumn]
    public class SpanCls
    {
        string text = String.Empty;
        string replaceable = String.Empty;
        string substitute = String.Empty;
        char[] arr = [];
        StringPool _strPool;

        public SpanCls()
        {
            text = "The quick brown fox jumps over the lazy dog 2. The quick brown fox jumps over the lazy dog 3. The quick brown fox jumps over the lazy dog 4. The quick brown fox jumps over the lazy dog 5. The quick brown fox jumps over the lazy dog 6. The quick brown fox jumps over the lazy dog 7. The quick brown fox jumps over the lazy dog 8. The quick brown fox jumps over the lazy dog 9. The quick brown fox jumps over the lazy dog";
           replaceable = "fox";
           substitute = "ELEPHANT";
            //Span<char> chars = SpanIndexOf(text, replaceable, substitute);
            //replaceable = "dog";
            //substitute = "WHILE";
            ////chars = SpanIndexOf(chars.ToString(), replaceable, substitute);
            //replaceable = "quick";
            //substitute = "big";
            arr = text.ToArray();
            _strPool = new StringPool(2048);
        }
        //public  Span<char> SpanIndexOf(in string text, in string replaceable, in string substitute)
        //[Benchmark]
        public void SpanIndexOf()
        {
            //string text = "Hello, World";
            //string text = "The quick brown fox jumps over the lazy dog";
            //string replaceable = "fox";
            //string substitute = "ELEPHANT";
            int startPosition = 0;
            bool isFound = false;
            //int countHit = 0;
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(arr);
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();

            //isFound = chars.ContainsAny(rSpan);
            startPosition = chars.IndexOf(rSpan);
            if (startPosition >= 0)
            {
                //Console.WriteLine($"isFound={isFound}");
                startPosition = chars.IndexOf(rSpan);
                //Console.WriteLine($"startPosition={startPosition}");
                //var lastPosition = chars.LastIndexOf(rSpan);
                //Console.WriteLine($"lastPosition={lastPosition}");
                char[] newCars = new char[chars.Length - rSpan.Length + sSpan.Length];
                Span<char> newSpan = new Span<char>(newCars);
                //Console.WriteLine($"newSpan.Length={newSpan.Length}");
                for (int i = 0; i < startPosition; i++)
                {
                    newSpan[i] = chars[i];
                }
                //Console.WriteLine($"1 newSpan={newSpan.ToString()}");
                for (int j = 0; j < sSpan.Length; j++)
                {
                    newSpan[j + startPosition] = sSpan[j];
                }
                //Console.WriteLine($"2 newSpan={newSpan.ToString()}");
                for (int j = startPosition + rSpan.Length; j < chars.Length; j++)
                {
                    newSpan[j - rSpan.Length + sSpan.Length] = chars[j];
                }
                //Console.WriteLine($"3 newSpan={newSpan.ToString()}");
                //return newSpan;
            }
            //return chars;
        }

        //[Benchmark]
        public void SpanIndexOfWithArrayPool()
        {
            //string text = "Hello, World";
            //string text = "The quick brown fox jumps over the lazy dog";
            //string replaceable = "fox";
            //string substitute = "ELEPHANT";
            int startPosition = 0;
            bool isFound = false;
            //int countHit = 0;
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(arr);
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();

            //isFound = chars.ContainsAny(rSpan);
            startPosition = chars.IndexOf(rSpan);
            if (startPosition >= 0)
            {
                //Console.WriteLine($"isFound={isFound}");
                startPosition = chars.IndexOf(rSpan);
                //Console.WriteLine($"startPosition={startPosition}");
                //var lastPosition = chars.LastIndexOf(rSpan);
                //Console.WriteLine($"lastPosition={lastPosition}");
                //char[] newCars = new char[chars.Length - rSpan.Length + sSpan.Length];
                var pool = ArrayPool<char>.Shared;
                char[] newChars = pool.Rent(chars.Length - rSpan.Length + sSpan.Length);
                Span<char> newSpan = new Span<char>(newChars);
                //Console.WriteLine($"newSpan.Length={newSpan.Length}");
                for (int i = 0; i < startPosition; i++)
                {
                    newSpan[i] = chars[i];
                }
                //Console.WriteLine($"1 newSpan={newSpan.ToString()}");
                for (int j = 0; j < sSpan.Length; j++)
                {
                    newSpan[j + startPosition] = sSpan[j];
                }
                //Console.WriteLine($"2 newSpan={newSpan.ToString()}");
                for (int j = startPosition + rSpan.Length; j < chars.Length; j++)
                {
                    newSpan[j - rSpan.Length + sSpan.Length] = chars[j];
                }

                pool.Return(newChars);
                //Console.WriteLine($"3 newSpan={newSpan.ToString()}");
                //return newSpan;
            }
            //return chars;
        }

        [Benchmark]
        public void SpanIndexOfWithArrayPoolCount()
        {
            int startPosition = 0;
            //bool isFound = false;
            
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(arr);
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();
            int countHit = chars.Count(rSpan);
            //Console.WriteLine($"countHit={countHit}");
            var pool = ArrayPool<char>.Shared;

            while (countHit > 0)
            {
                //isFound = chars.ContainsAny(rSpan);
                startPosition = chars.IndexOf(rSpan);
                if (startPosition >= 0)
                {
                    //Console.WriteLine($"isFound={isFound}");
                    //startPosition = chars.IndexOf(rSpan);
                    //Console.WriteLine($"startPosition={startPosition}");
                    //var lastPosition = chars.LastIndexOf(rSpan);
                    //Console.WriteLine($"lastPosition={lastPosition}");
                    //char[] newCars = new char[chars.Length - rSpan.Length + sSpan.Length];
                    
                    char[] newChars = pool.Rent(chars.Length - rSpan.Length + sSpan.Length);
                    Span<char> newSpan = new Span<char>(newChars);
                    //Console.WriteLine($"newSpan.Length={newSpan.Length}");
                    for (int i = 0; i < startPosition; i++)
                    {
                        newSpan[i] = chars[i];
                    }
                    //Console.WriteLine($"1 newSpan={newSpan.ToString()}");
                    for (int j = 0; j < sSpan.Length; j++)
                    {
                        newSpan[j + startPosition] = sSpan[j];
                    }
                    //Console.WriteLine($"2 newSpan={newSpan.ToString()}");
                    for (int j = startPosition + rSpan.Length; j < chars.Length; j++)
                    {
                        newSpan[j - rSpan.Length + sSpan.Length] = chars[j];
                    }

                    chars = new Span<char>(newChars);
                    pool.Return(newChars);
                    //Console.WriteLine($"3 newSpan={newSpan.ToString()}");
                    //return newSpan;
                    
                    //newChars.CopyTo(chars);
                }

                countHit--;
            }
            //return chars;
        }

        //[Benchmark]
        public void SpanIndexOfWithArrayPoolExtinsion()
        {
            //string text = "Hello, World";
            //string text = "The quick brown fox jumps over the lazy dog";
            //string replaceable = "fox";
            //string substitute = "ELEPHANT";
            int startPosition = 0;
            bool isFound = false;
            //int countHit = 0;
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(arr);
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();

            //isFound = chars.ContainsAny(rSpan);
            startPosition = chars.IndexOf(rSpan);
            if (startPosition >= 0)
            {
                //Console.WriteLine($"isFound={isFound}");
                startPosition = chars.IndexOf(rSpan);
                //Console.WriteLine($"startPosition={startPosition}");
                //var lastPosition = chars.LastIndexOf(rSpan);
                //Console.WriteLine($"lastPosition={lastPosition}");
                //char[] newCars = new char[chars.Length - rSpan.Length + sSpan.Length];
                var pool = ArrayPool<char>.Shared;
                var arraySegmentHandle = pool.RentSegment<char>(chars.Length - rSpan.Length + sSpan.Length);
                Span<char> newSpan = new Span<char>(arraySegmentHandle.Value.Array);
                //Console.WriteLine($"newSpan.Length={newSpan.Length}");
                for (int i = 0; i < startPosition; i++)
                {
                    newSpan[i] = chars[i];
                }
                //Console.WriteLine($"1 newSpan={newSpan.ToString()}");
                for (int j = 0; j < sSpan.Length; j++)
                {
                    newSpan[j + startPosition] = sSpan[j];
                }
                //Console.WriteLine($"2 newSpan={newSpan.ToString()}");
                for (int j = startPosition + rSpan.Length; j < chars.Length; j++)
                {
                    newSpan[j - rSpan.Length + sSpan.Length] = chars[j];
                }
                arraySegmentHandle.Dispose();
                //pool.Return(newChars);
                //Console.WriteLine($"3 newSpan={newSpan.ToString()}");
                //return newSpan;
            }
            //return chars;
        }

        //[Benchmark]
        public void SpanStandard()
        {
            //string text = "Hello, World";
            //string replaceable = "lla";
            //string substitute = "zz";
            int startPosition = 0;
            bool isFound = false;
            int countHit = 0;
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(arr);
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();
            //chars[0] = 'Z';
            for (int i = 0; i < chars.Length; i++)
            {
                //Console.WriteLine(chars[i]);
                for (int j = countHit; j < replaceable.Length; j++)
                {
                    if (chars[i] == replaceable[j])
                    {
                        if (startPosition == 0)
                            startPosition = i;
                        isFound = true;
                        countHit++;
                        //Console.WriteLine($"found one char - {chars[i]}, countHit={countHit}");
                        break;
                    }
                    else
                    {
                        if (countHit < replaceable.Length)
                        {
                            startPosition = 0;
                            isFound = false;
                            countHit = 0;
                            //Console.WriteLine($"clean Hit countHit={countHit}");
                        }
                    }
                }
            }
            //Console.WriteLine($"isFound={isFound}, countHit={countHit}, startPosition={startPosition} ");
            if (isFound && countHit == replaceable.Length)
            {
                //Console.WriteLine("It is OK. Will start to replace!");
                for (int j = 0; j < substitute.Length; j++)
                {
                    chars[startPosition + j] = substitute[j];
                }
                //for (int i = 0; i < chars.Length; i++)
                //{
                //    //Console.WriteLine(chars[i]);
                //}
            }
        }


        //public static void ArrayPoolFunc(in string text, in string replaceable, in string substitute)
        //{
        //    var cArr = text.ToCharArray();
        //    var pool = ArrayPool<char>.Shared.Rent(text.Length - replaceable.Length + substitute.Length);
        //    var span = pool.AsSpan<char>();
        //    pool[0] = 'Z';
        //    Console.WriteLine(pool[0]);
        //}

        [Benchmark]
        public void StringReplace()
        {
            string text2 = text.Replace(replaceable, substitute);
        }

        //[Benchmark]
        public void StringPoolz()
        {
            //var strPool = new StringPool(2048);
            //string textPool = StringPool.Shared.GetOrAdd(text);
            _strPool.Add(replaceable);
            var spanText = _strPool.GetOrAdd(text).Replace(replaceable, substitute).AsSpan();
            var text2 = _strPool.GetOrAdd(spanText).AsSpan();
            //Console.WriteLine(text2);
            //StringPool.Shared.Reset();
            _strPool.Reset();
        }
    }
}
