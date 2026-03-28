using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.Events;

public class StateTransition
{
    public Func<bool> CheckCondition;
    public EnemyState NextState;
    public string TriggerName = null;
}



