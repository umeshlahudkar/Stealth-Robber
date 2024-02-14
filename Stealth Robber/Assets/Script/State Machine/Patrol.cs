using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    private int currentPatrolIndex;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform[] _wayPoints) :
           base(_npc, _agent, _animator, _wayPoints)
    {
        stateType = StateType.Idle;
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool("walk", true);
        currentPatrolIndex = 0;
        agent.isStopped = false;
        agent.speed = 2;

        agent.SetDestination(wayPoints[currentPatrolIndex].position);
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % wayPoints.Length;
            agent.SetDestination(wayPoints[currentPatrolIndex].position);
        }
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("walk", false);
    }
}
