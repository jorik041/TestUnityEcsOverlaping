using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int CountX;
    public int CountY;
    public GameObject ObstaclePrefab;
    public GameObject AgentPrefab;

    private List<Transform> _agents;
    // Start is called before the first frame update
    void Start()
    {
        for (var x = 0; x < CountX; x++)
        {
            for (var y = 0; y < CountY; y++)
            {
                var xPos = noise.cnoise(new float2(y, x) * 0.21F) * 8;
                var yPos = noise.cnoise(new float2(x, y) * 0.21F) * 8;
                var position = new Vector3(xPos,yPos,0);
                Instantiate(ObstaclePrefab, position, Quaternion.identity);
            }
        }

        _agents = new List<Transform>();
        for (var x = 0; x < CountX; x++)
        {
            for (var y = 0; y < CountY; y++)
            {
                var xPos = noise.cnoise(new float2(y, x) * 0.21F) * 8;
                var yPos = noise.cnoise(new float2(x, y) * 0.21F) * 8;
                var position = new Vector3(xPos,yPos,0);//transform.TransformPoint(xPos, yPos, 1);
                _agents.Add(Instantiate(AgentPrefab, position, Quaternion.identity).transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var index = 0;
        for (var x = 0; x < CountX; x++)
        {
            for (var y = 0; y < CountY; y++)
            {
                var xPos = noise.cnoise(new float2(y, x) * 0.01F);
                var yPos = noise.cnoise(new float2(x, y) * 0.01F);
                var agent = _agents[index];
                agent.position = new Vector3(agent.position.x + xPos,agent.position.y + yPos,0);
                index++;
            }
        }
    }
}
