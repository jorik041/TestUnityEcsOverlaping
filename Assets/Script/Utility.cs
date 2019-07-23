using Unity.Mathematics;

namespace Script
{
    public static class Utility
    {
        public const int CellSize = 5;
        
        public static float3 GetQuadrantLowerLeftPointByPosition(float3 position)
        {
            return new float3(math.floor(position.x / CellSize) * CellSize, math.floor(position.y / CellSize) * CellSize, 0);
        }

        public static int GetQuadrantHashByPosition(float3 position, int cellSizes)
        {
            return GetQuadrantHashByPosition(new float2(position.x, position.y), cellSizes);
        }

        public static int GetQuadrantHashByPosition(float2 position, int cellSizes)
        {
            return Hash(Quantize(position, cellSizes));
        }

        private static int2 Quantize(float2 position, int cellSizes)
        {
            return new int2(math.floor(position / cellSizes));
        }

        private static int Hash(int2 grid)
        {
            // Simple int3 hash based on a pseudo mix of :
            // 1) https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
            // 2) https://en.wikipedia.org/wiki/Jenkins_hash_function
            var hash = grid.x;
            hash = (hash * 397) ^ grid.y;
            hash += hash << 3;
            hash ^= hash >> 11;
            hash += hash << 15;
            return hash;
        }
    }
}