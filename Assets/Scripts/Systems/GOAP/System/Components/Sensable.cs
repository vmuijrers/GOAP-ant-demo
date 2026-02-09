using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensable : MonoBehaviour, ISensorableEffect
{
    [SerializeField] private List<EffectState> effectStatesChangesOnEnter;
    [SerializeField] private List<EffectState> effectStatesChangesOnExit;

    public List<EffectState> GetEffectStatesOnEnter()
    {
        return effectStatesChangesOnEnter;
    }

    public List<EffectState> GetEffectStatesOnExit()
    {
        return effectStatesChangesOnExit;
    }

    public bool HasEffectsOnEnter(List<EffectState> effectStates)
    {
        return effectStates.TrueForAll(x => effectStatesChangesOnEnter.Contains(x));
    }
}
