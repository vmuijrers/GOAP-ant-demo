using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public int cost;
    public List<EffectState> preconditions = new List<EffectState>();
    public List<EffectState> postconditions = new List<EffectState>();

    protected GOAPAgent agent;

    public abstract int GetCost();

    public abstract void Initialize(GOAPAgent actor);
    public abstract void SetupConditionsAndEffects();
    public abstract bool IsViable();

    public abstract void OnEnterAction();
    public abstract void OnUpdateAction();
    public abstract void OnExitAction();
    public abstract void OnActionCompleted();
    public abstract void OnActionFailed();
}
