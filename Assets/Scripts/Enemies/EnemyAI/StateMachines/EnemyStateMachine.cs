using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateMachine
{
    public EnemyController Enemy;
    public Dictionary<string, EnemyState> States = new();
    public Dictionary<string, StateMachineParameter> Parameters = new();
    public Dictionary<string, StateTransition> Transitions = new();
    public EnemyState ActiveState;

    public void SetParameterFloat(string name, float value)
    {
        if (Parameters[name].Type == ParameterType.Float)
        {
            Parameters[name].SetValueFloat(value);
        }
    }
    public void SetParameterInt(string name, int value)
    {
        if (Parameters[name].Type == ParameterType.Int)
        {
            Parameters[name].SetValueInt(value);
        }
    }
    public void SetParameterBool(string name, bool value)
    {
        if (Parameters[name].Type == ParameterType.Bool)
        {
            Parameters[name].SetValueBool(value);
        }
    }

    public void SetParameterTrigger(string name)
    {
        // Kiểm tra xem Key có tồn tại trong Dictionary không trước khi dùng
        if (!Parameters.ContainsKey(name))
        {
            Debug.LogWarning($"StateMachine: Parameter '{name}' không tồn tại!");
            return;
        }

        if (Parameters[name].Type == ParameterType.Trigger)
        {
            foreach (var transition in ActiveState.Transitions)
            {
                if (transition.TriggerName != null && transition.TriggerName.Equals(name))
                {
                    TransitionState(transition.NextState);
                }
            }
        }
    }

    public abstract void Start();
    public void Update()
    {
        foreach (var transition in ActiveState.Transitions)
        {
            if (transition.CheckCondition())
            {
                TransitionState(transition.NextState);
                return;
            }
        }
        ActiveState.Update();
    }

    public void TransitionState(EnemyState nextState)
    {
        ActiveState.Exit();
        ActiveState = nextState;
        ActiveState.Enter();
    }
}