using UnityEngine.Events;

public class StateMachineParameter
{
    public ParameterType Type;
    public float ValueFloat;
    public int ValueInt;
    public bool ValueBool;

    public StateMachineParameter(ParameterType type)
    {
        Type = type;
    }

    public void SetValueFloat(float value)
    {
        ValueFloat = value;
    }
    public void SetValueInt(int value)
    {
        ValueInt = value;
    }
    public void SetValueBool(bool value)
    {
        ValueBool = value;
    }
}

public enum ParameterType
{
    Float,
    Int,
    Bool,
    Trigger,
}