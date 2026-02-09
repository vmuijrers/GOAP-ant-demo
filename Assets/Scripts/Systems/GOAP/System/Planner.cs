using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;

public class Planner
{
    public static List<T> Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int num = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[num];
            list[num] = temp;
        }
        return list;
    }

    public static Stack<Action> PlanActions(State playerState, List<Action> availableActions, IEnumerable<IGoal> availableGoals, out IGoal outGoal)
    {
        outGoal = null;
        availableGoals = Shuffle<IGoal>(availableGoals.ToList()).OrderByDescending(g => g.Priority);
        foreach (IGoal goal in availableGoals)
        {
            if (goal.IsViable(playerState))
            {
                var stack = TryPlanWithAStar(goal, playerState, availableActions, ref outGoal);
                if (outGoal != null && stack.Count > 0)
                {
                    StringBuilder str = new StringBuilder();
                    foreach(var action in stack)
                    {
                        str.Append(action.name);
                        str.Append(", ");
                    }
                    Debug.Log($"Goal {goal.name} is viable, plan: {str}");
                    return stack;
                }
                Debug.Log($"Goal {goal.name} is viable but plan not found!");
            }
            Debug.Log($"Goal {goal.name} Not Viable!");
        }
        return null;
    }

    /// <summary>
    /// Based on Pseudocode from wiki: https://en.wikipedia.org/wiki/A*_search_algorithm
    /// Loop Safe!
    /// </summary>
    /// <param name="goalToReach"></param>
    /// <param name="currentState"></param>
    /// <param name="availableActions"></param>
    /// <param name="outGoal"></param>
    public static Stack<Action> TryPlanWithAStar(IGoal goalToReach, State currentState, List<Action> availableActions, ref IGoal outGoal)
    {
        List<Node> closedList = new List<Node>();
        List<Node> openList = new List<Node>();

        State s = new State();
        s.state = currentState.state;
        openList.Add(new Node(null, null, s, 0));
        Dictionary<Effect, int> gScore = new Dictionary<Effect, int>();
        gScore.Add(openList[0].state.state, 0);

        float loopsMade = 0;

        while (openList.Count > 0 && loopsMade < 50)
        {
            loopsMade++;
            Node curNode = openList.OrderBy((t) => t.cumulativeCost).FirstOrDefault();

            if (curNode.state.CheckIfEffectsArePresent(goalToReach.Postconditions))
            {
                outGoal = goalToReach;
                return ReconstructPath(curNode);
            }

            openList.Remove(curNode);
            closedList.Add(curNode);

            foreach (Action action in availableActions)
            {
                if (curNode.state.CheckIfEffectsArePresent(action.preconditions))
                {
                    Node n = curNode.CloneNode();
                    n.parentNode = curNode;
                    n.cumulativeCost += action.GetCost();
                    n.actionForThisNode = action;
                    n.state.AddEffectsToState(action.postconditions);
                    if (closedList.Contains(n, new NodeComparer()))
                    {
                        continue;
                    }
                    int tentativeScore = n.cumulativeCost;
                    if (!openList.Contains(n, new NodeComparer()))
                    {
                        openList.Add(n);
                    }
                    else if (tentativeScore >= gScore[n.state.state])
                    {
                        continue;
                    }
                    gScore[n.state.state] = tentativeScore;
                }

            }

        }
        return null;
    }

    public static Stack<Action> ReconstructPath(Node endNode)
    {
        Stack<Action> plan = new Stack<Action>();
        do
        {
            plan.Push(endNode.actionForThisNode);
            endNode = endNode.parentNode;
        } while (endNode != null && endNode.actionForThisNode != null);
        return plan;
    }
}

public class Node
{
    public Node parentNode;
    public Action actionForThisNode;
    public State state;
    public int cumulativeCost;
    public Node() { }
    public Node(Node parentNode, Action action, State state, int cumulativeCost)
    {

        this.state = state;
        actionForThisNode = action;
        this.parentNode = parentNode;
        this.cumulativeCost = cumulativeCost;
    }

    public void AddEffectsToState(List<EffectState> effects)
    {
        state.AddEffectsToState(effects);
    }

    public Node CloneNode()
    {
        Node n = (Node)this.MemberwiseClone();
        n.state = new State();
        n.state.state = this.state.state;
        return n;

    }
}

public class NodeComparer : IEqualityComparer<Node>
{
    public bool Equals(Node n1, Node n2)
    {
        return n1.state.CompareState(n2.state);
    }
    public int GetHashCode(Node n)
    {
        return n.GetHashCode();
    }

}


