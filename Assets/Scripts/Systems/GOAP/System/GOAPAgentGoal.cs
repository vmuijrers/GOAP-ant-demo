public abstract class GOAPAgentGoal : Goal<GOAPAgent>
{
    public override void Initialize(GOAPAgent agent)
    {
        this.owner = agent;
    }

    public override bool IsViable(State state)
    {
        return state.CheckIfEffectsArePresent(Preconditions);
    }
}
