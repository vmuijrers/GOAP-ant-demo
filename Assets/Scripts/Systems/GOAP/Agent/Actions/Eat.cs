using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EatAction", menuName = "Actions/EatAction")]
public class Eat : MoveAction
{
    public override GameObject GetMoveTarget()
    {
        return null;
    }

    public override void OnActionCompleted()
    {
        Destroy(moveTarget);
        base.OnActionCompleted();
    }
}
