using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
