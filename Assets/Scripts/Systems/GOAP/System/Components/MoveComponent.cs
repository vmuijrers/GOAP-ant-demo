using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveComponent : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject moveTarget;
    private Vector3 destination;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(GameObject target)
    {
        moveTarget = target;
        if(moveTarget != null)
        {
            destination = moveTarget.transform.position;
        }
    }

    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    public MoveStatus OnUpdate()
    {
        if(navMeshAgent.destination != destination)
        {
            navMeshAgent.SetDestination(destination);
        }
        if(navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            return MoveStatus.Failed;
        }
        if(!navMeshAgent.pathPending && navMeshAgent.hasPath && navMeshAgent.remainingDistance < .5f)
        {
            return MoveStatus.Success;
        }
        return MoveStatus.Pending;
    }
}

public enum MoveStatus
{
    Pending = 0,
    Failed = 1,
    Success = 2
}