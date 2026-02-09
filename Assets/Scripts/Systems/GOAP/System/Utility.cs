using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static IEnumerable<T> FindAllObjects<T>(Vector3 position, float range, LayerMask layer, System.Func<T, bool> filter = null) where T : Component
    {
        List<T> list = new List<T>();
        var cols = Physics.OverlapSphere(position, range, layer);
        foreach (var c in cols)
        {
            var comp = c.GetComponent<T>();
            if (comp != null)
            {
                list.Add(comp);
            }
        }
        return list.FindAll(x => filter != null ? filter(x) : x);
    }

    public static T GetNearest<T>(Vector3 position, IEnumerable<T> list) where T : Component
    {
        if(list.Count() == 0) { return default(T); }
        return list
            .OrderBy(x => Vector3.Distance(position, x.transform.position))
            .FirstOrDefault();
    }
}