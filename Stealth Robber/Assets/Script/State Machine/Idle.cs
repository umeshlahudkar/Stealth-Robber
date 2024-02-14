using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    private float autoStateChangeTime = 5f;
    private float elapcedTime = 0;

    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform[] _wayPoints) :
            base(_npc, _agent, _animator, _wayPoints)
    {
        stateType = StateType.Idle;
    }

    public override void Enter()
    {
        base.Enter();
        //animator.SetTrigger("idle");
        elapcedTime = autoStateChangeTime;
    }

    public override void Update()
    {
        elapcedTime -= Time.deltaTime;
        if(elapcedTime <= 0)
        {
            elapcedTime = 0;
            nextState = new Patrol(npc, agent, animator, wayPoints);
            eventType = EventType.Exit;
            Debug.Log("Changing state idle to some other state");
        }
    }

    public override void Exit()
    {
        base.Exit();
        //animator.ResetTrigger("idle");
        elapcedTime = autoStateChangeTime;
    }
}
