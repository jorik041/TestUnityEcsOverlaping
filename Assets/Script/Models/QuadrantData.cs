using Script.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace Script.Models
{
    public struct QuadrantData
    {
        public Entity Entity;
        public float3 Position;
        public float2 BoxCollider;
    }
    
    public struct QuadrantData2
    {
        public Entity Entity;
        public Translation2 Translation;
        public AABBRadiusComponent Radius;
    }
}