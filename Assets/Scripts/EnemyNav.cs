using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Vector3> targets = new List<Vector3>();

    void Start()
    {
        
    }

    void Update()
    {
        agent.SetDestination(targets[0]);
        
    }
}
