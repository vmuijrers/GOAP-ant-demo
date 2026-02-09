using UnityEngine;
/// <summary>
/// A MoveAction is a specific action which also needs a moveTarget in the world
/// </summary>
public abstract class MoveAction : Action
{
    protected GameObject moveTarget;

    public abstract GameObject GetMoveTarget();

    public override int GetCost()
    {
        if (moveTarget == null) { return cost; }
        return cost + Mathf.RoundToInt(Vector3.Distance(agent.transform.position, moveTarget.transform.position) * 0.5f);
    }

    public override void Initialize(GOAPAgent actor)
    {
        this.agent = actor;
        SetupConditionsAndEffects();
        SetMoveTarget(GetMoveTarget());
    }

    public override void SetupConditionsAndEffects() { }

    public virtual void SetMoveTarget(GameObject moveTarget)
    {
        this.moveTarget = moveTarget;
    }

    public override bool IsViable()
    {
        moveTarget = GetMoveTarget();
        return moveTarget != null;
    }

    public override void OnEnterAction()
    {
        SetMoveTarget(GetMoveTarget());

        if (moveTarget != null)
        {
            agent.MoveComponent.SetTarget(moveTarget);
        }
        else
        {
            OnActionFailed();
        }
    }
    public override void OnUpdateAction()
    {
        var result = agent.MoveComponent.OnUpdate();
        switch (result)
        {
            case MoveStatus.Failed: OnActionFailed(); break;
            case MoveStatus.Success: OnActionCompleted(); break;
        }
    }
    public override void OnExitAction()
    {
        SetMoveTarget(null);
    }

    public override void OnActionCompleted()
    {
        SetMoveTarget(null);
        agent.OnActionCompleted(this);
    }

    public override void OnActionFailed()
    {
        SetMoveTarget(null);
        agent.OnActionFailed(this);
    }
}