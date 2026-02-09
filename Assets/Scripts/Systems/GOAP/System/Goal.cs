using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal<T> : ScriptableObject, IGoal
{
    [SerializeField] protected int priority; //Higher value is more important
    public int Priority => priority;

    protected T owner;

    [field: SerializeField] public List<EffectState> Preconditions { get; protected set; } = new List<EffectState>();
    [field: SerializeField] public List<EffectState> Postconditions { get; protected set; } = new List<EffectState>();

    public abstract void Initialize(T agent);
    public abstract bool IsViable(State state);
}

public interface IGoal
{
    string name { get; }
    int Priority { get; }
    bool IsViable(State state);
    List<EffectState> Preconditions { get; }
    List<EffectState> Postconditions { get; }
}