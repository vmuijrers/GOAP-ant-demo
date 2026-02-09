using UnityEngine;

[CreateAssetMenu(fileName = "DropOffFood", menuName = "Actions/DropOffFood")]
public class DropOffFood : MoveAction
{
    public string DropOffTargetName = "Nest";

    public override GameObject GetMoveTarget()
    {
        return agent.Memory.GetValue<GameObject>(DropOffTargetName);
    }
}
