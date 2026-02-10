using UnityEngine;

[CreateAssetMenu(fileName = "Goal_", menuName = "Goals/ConcreteGoal")]
public class ConcreteGoal : GOAPAgentGoal
{
    public override bool IsViable(State state)
    {
        return true;
    }
}