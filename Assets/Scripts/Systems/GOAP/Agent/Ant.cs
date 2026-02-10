using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : GOAPAgent
{
    public GameObject Nest;

    [SerializeReference] public List<AdvancedEffectStateBase> effects = new List<AdvancedEffectStateBase>();
    [SerializeReference] public List<AdvancedEffectStateComparerBase> effectComparers = new List<AdvancedEffectStateComparerBase>();

    protected override void Start()
    {
        base.Start();
        State.SetValueInState("isAt", Nest);
        //var target = State.GetValueFromState<GameObject>("isAt");
        //if(target == Nest)
        //{
        //}
    }

    protected override void SetupMemory()
    {
        Memory.SetValue("Nest", Nest);
    }

    [ContextMenu("Add Float")]
    public void AddFloatToEffect()
    {
        effects.Add(new AdvancedEffectStateFloat("var", 0));
    }

    [ContextMenu("Add GameObject")]
    public void AddGameObjectToEffect()
    {
        effects.Add(new AdvancedEffectStateGameObject("var", null));
    }

    [ContextMenu("Add GameObject Comparer")]
    public void AddGameObjectToEffectComparer()
    {
        effectComparers.Add(new AdvancedEffectStateComparerGameObject());
    }

    [ContextMenu("Add Float Comparer")]
    public void AddFloatToEffectComparer()
    {
        effectComparers.Add(new AdvancedEffectStateComparerFloat());
    }
}
