using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [field: SerializeField] public float SightRange { get; private set; } = 10;
    [SerializeField] private float checkIntervalInSeconds = .25f;
    [SerializeField] private LayerMask checkLayerMask;

    public System.Action<List<EffectState>> OnEffectStateSpotted;
    private HashSet<ISensorableEffect> activeSensorableEffects;
    private HashSet<ISensorableEffect> gatheredSet;
    // Start is called before the first frame update
    void Start()
    {
        activeSensorableEffects = new HashSet<ISensorableEffect>();
        gatheredSet = new HashSet<ISensorableEffect>();
        InvokeRepeating("Check", 1f, checkIntervalInSeconds);
    }

    private void Check()
    {
        gatheredSet.Clear();
        var cols = Physics.OverlapSphere(transform.position, SightRange, checkLayerMask);
        foreach(var c in cols)
        {
            var comp = c.GetComponent<ISensorableEffect>();
            if(comp != null)
            {
                gatheredSet.Add(comp);
            }
        }

        foreach(var c in gatheredSet)
        {
            //Debug.Log("Entering: " + c);
            OnEffectStateSpotted?.Invoke(c.GetEffectStatesOnEnter());
        }

        foreach(var c in activeSensorableEffects)
        {
            if (gatheredSet.Contains(c))
            {
                continue;
            }
            //Debug.Log("Exiting: " + c);
            OnEffectStateSpotted?.Invoke(c.GetEffectStatesOnExit());
        }
        activeSensorableEffects.Clear();
        activeSensorableEffects.UnionWith(gatheredSet);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SightRange);
    }

}

public interface ISensorableEffect
{
    List<EffectState> GetEffectStatesOnEnter();
    List<EffectState> GetEffectStatesOnExit();
}