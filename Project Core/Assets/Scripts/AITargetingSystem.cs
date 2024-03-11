using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AITargetingSystem : MonoBehaviour
{
    public float memorySpan = 3.0f;

    AISensoryMemory memory = new AISensoryMemory(10);
    AISensor sensor;

    // Start is called before the first frame update
    void Start()
    {
        sensor = GetComponent<AISensor>();
    }

    // Update is called once per frame
    void Update()
    {
        memory.UpdateSenses(sensor);
        memory.ForgetMemories(memorySpan);
    }

    private void OnDrawGizmos()
    {
        foreach(var memory in memory.memories)
        {
            Color color = Color.red;
            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, 0.2f);
        }
    }
}
