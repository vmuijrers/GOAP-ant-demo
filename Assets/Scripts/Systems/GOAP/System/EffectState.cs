using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Flags]
public enum Effect
{
    NestHasFood         = 1 << 0,
    SelfHasFood         = 1 << 1,
    SelfIsThreatened    = 1 << 2,
    SelfNearPlayer      = 1 << 3,
    NestThreatened      = 1 << 4,
    SelfNearFoodSource  = 1 << 5,
    SelfIsHungry        = 1 << 6,
    SelfNearNest        = 1 << 7,
}

[System.Serializable]
public struct EffectState
{
    public Effect effect;
    public bool isActive;

    public EffectState(Effect effect, bool value)
    {
        this.effect = effect;
        this.isActive = value;
    }
}


[System.Serializable]
public class AdvancedEffectStateBase
{

}

[System.Serializable]
public class AdvancedEffectState<T> : AdvancedEffectStateBase
{
    public string name;
    public T value;
}

[System.Serializable]
public class AdvancedEffectStateFloat : AdvancedEffectState<float>
{
    public AdvancedEffectStateFloat(string name, float obj)
    {
        this.name = name;
        this.value = obj;
    }
}
[System.Serializable]
public class AdvancedEffectStateGameObject : AdvancedEffectState<GameObject>
{
    public AdvancedEffectStateGameObject(string name, GameObject obj)
    {
        this.name = name;
        this.value = obj;
    }
}
[System.Serializable]
public abstract class AdvancedEffectStateComparerBase
{
    public string name;
    public CompareType type;
}


public abstract class AdvancedEffectStateComparer<T> : AdvancedEffectStateComparerBase
{
    public T value;
    public abstract bool Compare(T target);
}

[System.Serializable]
public class AdvancedEffectStateComparerFloat : AdvancedEffectStateComparer<float>
{
    public override bool Compare(float target)
    {
        switch (type)
        {
            case CompareType.CompareEqual: return value == target;
            case CompareType.CompareLess: return value < target;
            case CompareType.CompareGreater: return value > target;
            case CompareType.CompareEqualOrLess: return value <= target;
            case CompareType.CompareEqualOrGreater: return value >= target;
        }
        return false;
    }
}

[System.Serializable]
public class AdvancedEffectStateComparerGameObject : AdvancedEffectStateComparer<GameObject>
{
    public override bool Compare(GameObject target)
    {
        switch (type)
        {
            case CompareType.CompareEqual: return value == target;
            case CompareType.CompareNotEqual: return value != target;
            default: return false;
        }
    }
}

public enum CompareType
{
    CompareEqual,
    CompareNotEqual,
    CompareLess,
    CompareGreater,
    CompareEqualOrLess,
    CompareEqualOrGreater
}

public class Trigger<T>
{
    public GameEventType eventType;
    public Condition<T> condition;
    public Action<T> action;

    public Trigger(GameEventType eventType, Condition<T> condition, Action<T> action)
    {
        this.eventType = eventType;
        this.condition = condition;
        this.action = action;
        Setup();
    }

    public void Setup()
    {
        //Message<T>.AddListener(eventType, HandleEvent);
    }

    public void HandleEvent(T target)
    {
        if (condition.Evaluate(target))
        {
            action?.Invoke(target);
        }
    }
}

public enum GameEventType
{

}

public class Condition<T>
{
    private List<Predicate<T>> conditions;

    public Condition(List<System.Predicate<T>> conditions)
    {
        this.conditions = conditions;
    }

    public bool Evaluate(T target)
    {
        if(conditions.ToList().TrueForAll((x) => x.Invoke(target)))
        {
            return true;
        }
        return false;
    }
}

