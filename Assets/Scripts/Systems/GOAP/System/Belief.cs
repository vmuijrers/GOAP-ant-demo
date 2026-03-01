using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOAPBelief
{
    public class AgentBelief
    {
        public string Name { get; } //TODO -> change to enum
        public System.Func<bool> condition;
        public System.Func<Vector3> observedLocation = () => Vector3.zero;

        public AgentBelief(string name)
        {
            Name = name;
        }

        public bool Evaluate() => condition();
        public Vector3 Location => observedLocation();

        // A simple builder 
        public class Builder
        {
            readonly AgentBelief belief;

            public Builder(string name)
            {
                belief = new AgentBelief(name);
            }

            public Builder WithCondition(System.Func<bool> condition)
            {
                belief.condition = condition;
                return this;
            }

            public Builder WithLocation(System.Func<Vector3> observedLocation)
            {
                belief.observedLocation = observedLocation;
                return this;
            }

            public AgentBelief Build()
            {
                return belief;
            }
        }
    }

    public class BeliefFactory
    {
        readonly Agent agent;
        readonly Dictionary<string, AgentBelief> beliefs;

        public BeliefFactory(Agent agent, Dictionary<string, AgentBelief> beliefs)
        {
            this.agent = agent;
            this.beliefs = beliefs;
        }

        public void AddBelief(string key, Func<bool> condition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(condition)
                .Build());
        }

        //public void AddSensorBelief(string key, Sensor sensor)
        //{
        //    beliefs.Add(key, new AgentBelief.Builder(key)
        //        .WithCondition(() => sensor.IsTargetInRange)
        //        .WithLocation(() => sensor.TargetPosition)
        //        .Build());
        //}

        public void AddLocationBelief(string key, float distance, Transform locationCondition)
        {
            AddLocationBelief(key, distance, locationCondition.position);
        }

        public void AddLocationBelief(string key, float distance, Vector3 locationCondition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => InRangeOf(locationCondition, distance))
                .WithLocation(() => locationCondition)
                .Build());
        }

        bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(agent.transform.position, pos) < range;
    }

    public class AgentAction
    {
        public string Name { get; }
        public float Cost { get; private set; }

        public HashSet<AgentBelief> Preconditions { get; } = new();
        public HashSet<AgentBelief> Effects { get; } = new();

        IActionStrategy strategy;
        public bool Complete => strategy.Complete;

        public AgentAction(string name)
        {
            Name = name;
        }

        public void Start() => strategy.Start();

        public void Update(float deltaTime)
        {
            // Check if the action can be performed and update the strategy
            if (strategy.CanPerform)
            {
                strategy.Update(deltaTime);
            }

            // Bail out if the strategy is still executing
            if (!strategy.Complete) return;

            // Apply effects
            foreach (var effect in Effects)
            {
                effect.Evaluate();
            }
        }

        public void Stop() => strategy.Stop();

        public class Builder
        {
            readonly AgentAction action;

            public Builder(string name)
            {
                action = new AgentAction(name)
                {
                    Cost = 1
                };
            }

            public Builder WithCost(float cost)
            {
                action.Cost = cost;
                return this;
            }

            public Builder WithStrategy(IActionStrategy strategy)
            {
                action.strategy = strategy;
                return this;
            }

            public Builder AddPrecondition(AgentBelief precondition)
            {
                action.Preconditions.Add(precondition);
                return this;
            }

            public Builder AddEffect(AgentBelief effect)
            {
                action.Effects.Add(effect);
                return this;
            }

            public AgentAction Build()
            {
                return action;
            }
        }

        public interface IActionStrategy
        {
            bool CanPerform { get; }
            bool Complete { get; }

            void Start()
            {
                // noop
            }

            void Update(float deltaTime)
            {
                // noop
            }

            void Stop()
            {
                // noop
            }
        }

    }

    public class AgentGoal
    {
        public string Name { get; }
        public float Priority { get; private set; }
        public HashSet<AgentBelief> DesiredEffects { get; } = new();

        public AgentGoal(string name)
        {
            Name = name;
        }

        public class Builder
        {
            readonly AgentGoal goal;

            public Builder(string name)
            {
                goal = new AgentGoal(name);
            }

            public Builder WithPriority(float priority)
            {
                goal.Priority = priority;
                return this;
            }

            public Builder WithDesiredEffect(AgentBelief effect)
            {
                goal.DesiredEffects.Add(effect);
                return this;
            }

            public AgentGoal Build()
            {
                return goal;
            }
        }
    }

    public class Agent : MonoBehaviour
    {
        public HashSet<AgentAction> actions;
        public HashSet<AgentGoal> goals;
        
        public Dictionary<string, AgentBelief> beliefs = new(); // These may or may not be true
        public BeliefFactory beliefFactory;

        public float Health { get; private set; } = 100;
        public GameObject Target { get; private set; } = null;

        public void Start()
        {
            
        }

        public void SetupBeliefs()
        {
            beliefs = new();
            beliefFactory = new BeliefFactory(this, beliefs);

            beliefFactory.AddBelief("Nothing", () => false);
            beliefFactory.AddBelief("Low Health", () => Health < 30);
            beliefFactory.AddBelief("Has Target", () => Target != null);
        }

        public void SetupGoals()
        {

        }

        public void SetupActions()
        {
            actions = new HashSet<AgentAction>();

            actions.Add(new AgentAction.Builder("Pickup Resource")
                .WithStrategy(new HealStrategy())
                .AddPrecondition(beliefs["Low Health"])
                .AddEffect(beliefs["High Health"])
                .Build());
        }
    }

    public class HealStrategy : AgentAction.IActionStrategy
    {
        public bool CanPerform => throw new NotImplementedException();

        public bool Complete => throw new NotImplementedException();
    }
}
