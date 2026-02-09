using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wander", menuName = "Actions/Wander")]
public class Wander : MoveAction
{
    public float wanderDistance = 10;

    public override bool IsViable()
    {
        return agent.State.CheckIfEffectIsNotPresent(Effect.SelfIsThreatened);
    }

    public override void OnEnterAction()
    {
        MoveToRandomPoint();
    }

    public override void OnUpdateAction()
    {
        var result = agent.MoveComponent.OnUpdate();
        switch (result)
        {
            case MoveStatus.Failed: OnActionFailed(); break;
            case MoveStatus.Success: MoveToRandomPoint(); break;
        }

        if (agent.State.CheckIfEffectsArePresent(postconditions))
        {
            OnActionCompleted();
        }
    }

    private void MoveToRandomPoint()
    {
        agent.MoveComponent.SetDestination(agent.transform.position + Random.insideUnitSphere.normalized * wanderDistance);
    }

    public override GameObject GetMoveTarget()
    {
        return null;
    }
}
