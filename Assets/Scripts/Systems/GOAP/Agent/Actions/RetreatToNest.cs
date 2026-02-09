using UnityEngine;
[CreateAssetMenu(fileName = "RetreatToNest", menuName = "Actions/RetreatToNest")]
public class RetreatToNest : MoveAction
{
    public override GameObject GetMoveTarget()
    {
        return agent.Memory.GetValue<GameObject>("Nest");
    }
}
