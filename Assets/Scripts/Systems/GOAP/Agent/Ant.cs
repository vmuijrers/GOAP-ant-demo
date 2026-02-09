using System.Collections;
using UnityEngine;

public class Ant : GOAPAgent
{
    public GameObject Nest;

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetupMemory()
    {
        Memory.SetValue("Nest", Nest);
    }
}
