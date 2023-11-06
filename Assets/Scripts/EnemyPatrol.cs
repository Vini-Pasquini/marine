using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public float moveSpeed = 2.0f;

    private int currentPatrolIndex = 0;
    private Transform currentPatrolPoint;

    void Start()
    {
        if (patrolPoints.Count > 0)
        {
            currentPatrolPoint = patrolPoints[0];
        }
    }

    void Update()
    {
        if (currentPatrolPoint == null)
            return;

        // Move o inimigo em direção ao ponto de patrulha atual
        transform.position = Vector3.MoveTowards(transform.position, currentPatrolPoint.position, moveSpeed * Time.deltaTime);

        // Se o inimigo alcançar o ponto de patrulha atual, avance para o próximo ponto
        if (Vector3.Distance(transform.position, currentPatrolPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            currentPatrolPoint = patrolPoints[currentPatrolIndex];
        }
    }
}
