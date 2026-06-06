using System.Collections.Generic;

namespace ECS
{
    public class IdPool
    {
        public uint Count { get; private set; }

        private readonly Queue<uint> pool = new();

        public uint Get()
        {
            bool recycled = pool.TryDequeue(out uint id);
            id = recycled ? id : ++Count;
            return id;
        }

        public void Return(uint id)
        {
            pool.Enqueue(id);
        }
    }
}