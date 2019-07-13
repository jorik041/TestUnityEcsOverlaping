using Script;
using Script.Systems;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

//[DisableAutoCreation]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class DebugQuadrantSystem : ComponentSystem
{
    private QuadrantSystem _quadrantSystem;
    private Camera _camera;

    protected override void OnUpdate()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            return;
        }
        
        DrawQuadrant(_camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private void DrawQuadrant(float3 position)
    {
        Vector3 lowerLeftPoint = Utility.GetQuadrantLowerLeftPointByPosition(position);
        Debug.DrawLine(lowerLeftPoint, lowerLeftPoint + new Vector3(1, 0) * Utility.CellSize);
        Debug.DrawLine(lowerLeftPoint, lowerLeftPoint + new Vector3(0, 1) * Utility.CellSize);
        Debug.DrawLine(lowerLeftPoint + new Vector3(1, 0) * Utility.CellSize, lowerLeftPoint + new Vector3(1, 1) * Utility.CellSize);
        Debug.DrawLine(lowerLeftPoint + new Vector3(0, 1) * Utility.CellSize, lowerLeftPoint + new Vector3(1, 1) * Utility.CellSize);
    }
}