using UnityEngine;

[CreateAssetMenu(fileName = "PickupFoodAction", menuName = "Actions/PickupFoodAction")]
public class PickupFood : MoveAction
{
    public LayerMask FoodLayer;

    public override void OnActionCompleted()
    {
        Destroy(moveTarget);
        base.OnActionCompleted();
    }

    public override GameObject GetMoveTarget()
    {
        return Utility.GetNearest(agent.transform.position, Utility.FindAllObjects<Transform>(agent.transform.position, agent.Sensor.SightRange, FoodLayer)).gameObject;
    }
}
