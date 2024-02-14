using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform[] wayPoints;

    private State currentState;

    private void Start()
    {
        currentState = new Idle(gameObject, agent, animator, wayPoints);
    }

    private void Update()
    {
        currentState = currentState.Process();
    }
}
