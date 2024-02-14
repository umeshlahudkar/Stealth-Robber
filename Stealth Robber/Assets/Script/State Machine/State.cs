using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum StateType
    {
        Idle, Patrol, Persue, Attack
    }

    public enum EventType
    {
        Enter,
        Update,
        Exit
    }

    protected StateType stateType;
    protected EventType eventType;
    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected State nextState;

    protected Transform[] wayPoints;

    private float visDistance = 10.0f;
    private float visAngle = 60f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform[] _wayPoints)
    {
        npc = _npc;
        agent = _agent;
        animator = _animator;
        wayPoints = _wayPoints;

        eventType = EventType.Enter;
    }

    public virtual void Enter() { eventType = EventType.Update; }
    public virtual void Update() { eventType = EventType.Update; }
    public virtual void Exit() { eventType = EventType.Exit; }

    public State Process()
    {
        if (eventType == EventType.Enter) Enter();
        if (eventType == EventType.Update) Update();
        if (eventType == EventType.Exit) 
        {
            Exit();
            return nextState;
        }

        return this;
    }
}
