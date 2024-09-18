using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayPoolExtensions
{
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
