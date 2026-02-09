using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FleeAction", menuName = "Actions/FleeAction")]
public class Flee : MoveAction
{
    public float fleeDistance = 15;
    public LayerMask ThreatLayer;
    private GameObject threatTarget;

    public override void Initialize(GOAPAgent actor)
    {
        base.Initialize(actor);
    }

    public override bool IsViable()
    {
        return agent.State.CheckIfEffectsArePresent(preconditions);
    }

    private GameObject GetThreatTarget()
    {
        return Utility.GetNearest(agent.transform.position,
            Utility.FindAllObjects<Sensable>(agent.transform.position, agent.Sensor.SightRange, ThreatLayer, (x) =>
            {
                return x.HasEffectsOnEnter(new List<EffectState>()
                {
                    new EffectState()
                    {
                        effect = Effect.SelfIsThreatened,
                        isActive = true
                    }
                });
            }))?.gameObject;
    }

    public override void OnEnterAction()
    {
        threatTarget = GetThreatTarget();
        if(threatTarget != null)
        {
            agent.MoveComponent.SetDestination(threatTarget.transform.position + (agent.transform.position - threatTarget.transform.position).normalized * fleeDistance);
        }
        else
        {
            OnActionCompleted();
        }
    }

    public override GameObject GetMoveTarget()
    {
        return null;
    }
}
