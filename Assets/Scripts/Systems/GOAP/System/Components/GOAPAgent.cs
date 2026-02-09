using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class GOAPAgent : MonoBehaviour
{
    [field: SerializeField] public State State { get; private set; }
    [SerializeField] protected List<GOAPAgentGoal> allGoals = new List<GOAPAgentGoal>();
    [SerializeField] protected List<Action> allActions = new List<Action>();
    [SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [SerializeField] public MoveComponent MoveComponent { get; private set; }
    protected IGoal currentGoal;
    protected Action currentAction;

    public AgentMemory Memory { get; private set; }

    private Sensor sensor;
    public Sensor Sensor => sensor ?? GetComponent<Sensor>();

    public Stack<Action> actionStack = new Stack<Action>();
    private List<IGoal> allGoalsInterfaced = new List<IGoal>();

    protected virtual void Start()
    {
        Memory = new AgentMemory();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<Sensor>();
        MoveComponent = GetComponent<MoveComponent>();
        SetupMemory();
        InitializeActionsAndGoals();
    }

    protected abstract void SetupMemory();

    private void OnEnable()
    {
        Sensor.OnEffectStateSpotted += HandleEffectStateSpotted;
    }

    private void OnDisable()
    {
        Sensor.OnEffectStateSpotted -= HandleEffectStateSpotted;
    }

    private void HandleEffectStateSpotted(List<EffectState> list)
    {
        //foreach(var c in list)
        //{
        //    Debug.Log($"Added Effects: {c.effect}: {c.isActive}");
        //}
        State.AddEffectsToState(list);
    }

    protected virtual void InitializeActionsAndGoals()
    {
        List<Action> initializedActions = new List<Action>();
        for(int i = 0; i < allActions.Count;i++)
        {
            var action = Instantiate(allActions[i]);
            action.Initialize(this);
            initializedActions.Add(action);
        }
        allActions = initializedActions;
        
        List<GOAPAgentGoal> initializedGoals = new List<GOAPAgentGoal>();
        for (int i = 0; i < allGoals.Count; i++)
        {
            var goal = Instantiate(allGoals[i]);
            goal.Initialize(this);
            initializedGoals.Add(goal);
        }
        allGoals = initializedGoals;
        allGoalsInterfaced = allGoals.Cast<IGoal>().ToList();
    }

    protected virtual void Update()
    {
        ExecuteGoal();
    }

    public void ExecuteGoal()
    {
        if (currentGoal == null || !currentGoal.IsViable(State) || (currentAction != null && !currentAction.IsViable()))
        {
            ReplanGoal();

        }
        else if (currentAction == null)
        {
            GetNextAction();
        }
        else
        {
            currentAction.OnUpdateAction();
        }
    }

    public void OnActionCompleted(Action action)
    {
        State.AddEffectsToState(action.postconditions);
        action.OnExitAction();
        GetNextAction();
    }

    public void OnActionFailed(Action action)
    {
        action.OnExitAction();
        currentAction = null;
    }

    public void GetNextAction()
    {
        if (currentGoal != null && actionStack.Count > 0)
        {
            currentAction = actionStack.Pop();
            if (!currentAction.IsViable())
            {
                ReplanGoal();
            }
            currentAction?.OnEnterAction();
        }
        else
        {
            //CurrentGoal Completed Succesfully! Replan!
            ReplanGoal();
        }
    }

    public void ReplanGoal()
    {
        if (currentAction != null)
        {
            currentAction.OnExitAction();
        }
        currentAction = null;
        currentGoal = null;
        actionStack = Planner.PlanActions(State, allActions, allGoalsInterfaced, out currentGoal);
    }
}
