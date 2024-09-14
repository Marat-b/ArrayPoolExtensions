using System.Buffers;

namespace ArrayPoolExtensions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SpanIndexOf();
        }

        public static void SpanStandard()
        {
            string text = "Hello, World";
            string replaceable = "lla";
            string substitute = "zz";
            int startPosition = 0;
            bool isFound = false;
            int countHit = 0;
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(text.ToArray());
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();
            chars[0] = 'Z';
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
                        Console.WriteLine($"found one char - {chars[i]}, countHit={countHit}");
                        break;
                    }
                    else
                    {
                        if (countHit < replaceable.Length)
                        {
                            startPosition = 0;
                            isFound = false;
                            countHit = 0;
                            Console.WriteLine($"clean Hit countHit={countHit}");
                        }
                    }
                }
            }
            Console.WriteLine($"isFound={isFound}, countHit={countHit}, startPosition={startPosition} ");
            if (isFound && countHit == replaceable.Length)
            {
                Console.WriteLine("It is OK. Will start to replace!");
                for (int j = 0; j < substitute.Length; j++)
                {
                    chars[startPosition + j] = substitute[j];
                }
                for (int i = 0; i < chars.Length; i++)
                {
                    Console.WriteLine(chars[i]);
                }
            }
        }
        public static void SpanIndexOf()
        {
            string text = "Hello, World";
            string replaceable = "or";
            string substitute = "z";
            int startPosition = 0;
            bool isFound = false;
            //int countHit = 0;
            //ReadOnlySpan<char> chars = text.AsSpan();
            Span<char> chars = new Span<char>(text.ToArray());
            ReadOnlySpan<char> rSpan = replaceable.AsSpan();
            ReadOnlySpan<char> sSpan = substitute.AsSpan();

            isFound = chars.ContainsAny(rSpan);
            if (isFound)
            {
                Console.WriteLine($"isFound={isFound}");
                startPosition = chars.IndexOf(rSpan);
                Console.WriteLine($"startPosition={startPosition}");
                //var lastPosition = chars.LastIndexOf(rSpan);
                //Console.WriteLine($"lastPosition={lastPosition}");
                char[] newCars = new char[chars.Length - rSpan.Length + sSpan.Length];
                Span<char> newSpan = new Span<char>(newCars);
                Console.WriteLine($"newSpan.Length={newSpan.Length}");
                for (int i = 0; i < startPosition; i++)
                {
                    newSpan[i] = chars[i];
                }
                Console.WriteLine($"newSpan={newSpan.ToString()}");
                for (int j = 0; j < sSpan.Length; j++)
                {
                    newSpan[j + startPosition] = sSpan[j];
                }
                Console.WriteLine($"newSpan={newSpan.ToString()}");
                for (int j = startPosition + rSpan.Length; j < chars.Length; j++)
                {
                    newSpan[j - rSpan.Length + sSpan.Length] = chars[j];
                }
                Console.WriteLine($"newSpan={newSpan.ToString()}");
            }
        }
    }


    

    

    public static class ArrayPoolExtensions
    {
        public static ArraySegmentHandle<T> RentSegment<T>(this ArrayPool<T> pool, int length)
        {
            return new ArraySegmentHandle<T>(pool, length);
        }
    }
    public readonly struct ArraySegmentHandle<T> : IDisposable
    {
        private readonly ArrayPool<T> _pool;

        public ArraySegmentHandle(ArrayPool<T> pool, int length)
        {
            _pool = pool;
            Value = new ArraySegment<T>(pool.Rent(length), 0, length);
        }

        public ArraySegment<T> Value { get; }

        public void Dispose()
        {
            _pool.Return(Value.Array!);
        }
    }

    public static class MemoryPoolExtensions
    {
        public static MemorySegmentHandle<T> RentMemory<T>(this MemoryPool<T> pool, int length)
        {
            return new MemorySegmentHandle<T>(pool, length);
        }
    }
    public readonly struct MemorySegmentHandle<T> : IDisposable
    {
        private readonly MemoryPool<T> _pool;

        public MemorySegmentHandle(MemoryPool<T> pool, int length)
        {
            _pool = pool;
            //var memoryOwner = pool.Rent(length);
            //memoryOwner.
            Value = pool.Rent(length).Memory;
        }

        public Memory<T> Value { get; }

        public void Dispose()
        {
            //_pool.Return(Value.Array!);
            _pool.Dispose();
        }
    }
}
