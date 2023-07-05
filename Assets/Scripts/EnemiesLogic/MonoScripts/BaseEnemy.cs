using System;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    protected Action Action { get; private set; }

    public int Entity { get; private set; }

    public void Init(int entity, Action action)
    {
        Entity = entity;
        Action += action;
    }

    internal void Death()
    {
        Action?.Invoke();
    }
}