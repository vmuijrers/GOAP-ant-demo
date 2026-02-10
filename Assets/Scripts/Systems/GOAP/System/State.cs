using System.Collections.Generic;
using System;

[System.Serializable]
public class State
{
    public Effect state;
    public Dictionary<string, object> DictionaryState { get; private set; } = new Dictionary<string, object>();

    public State() { }

    //public bool CompareState(State otherState)
    //{
    //    foreach(var vk in DictionaryState)
    //    {
    //        if (!otherState.DictionaryState.ContainsKey(vk.Key)) return false;
    //        if (vk.Value != otherState.DictionaryState[vk.Key]) return false;
    //    }
    //    return true;
    //}

    public T GetValueFromState<T>(string key)
    {
        if (DictionaryState.ContainsKey(key))
        {
            return (T)DictionaryState[key];
        }
        return default(T);
    }

    public void SetValueInState<T>(string key, T value)
    {
        if (!DictionaryState.ContainsKey(key))
        {
            DictionaryState.Add(key, default(T));
        }
        DictionaryState[key] = value;
    }

    public void AddEffectsToState(IEnumerable<EffectState> effects)
    {
        foreach (EffectState efState in effects)
        {
            if (efState.isActive)
            {
                AddEffect(efState.effect);
            }
            else
            {
                RemoveEffect(efState.effect);
            }
        }
    }

    public void AddEffect(Effect effect)
    {
        state |= effect;
    }

    public void RemoveEffect(Effect effect)
    {
        state &= ~effect;
    }

    public bool CheckIfEffectsArePresent(List<EffectState> effects)
    {
        if(effects == null || effects.Count == 0) { return true; }
        bool res = true;
        foreach (EffectState efState in effects)
        {
            res &= efState.isActive ? CheckIfEffectIsPresent(efState.effect) : CheckIfEffectIsNotPresent(efState.effect);
        }
        return res;
    }

    public bool CheckIfEffectIsPresent(Effect effect)
    {
        return (state & effect) != 0;
    }

    public bool CheckIfEffectIsNotPresent(Effect effect)
    {
        return !((state & effect) != 0);
    }

    public bool CompareState(State otherState)
    {
        return (state == otherState.state);
    }

}


