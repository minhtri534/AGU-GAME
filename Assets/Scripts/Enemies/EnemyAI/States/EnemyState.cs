using System.Collections.Generic;

public abstract class EnemyState
{
    public EnemyController enemy;
    public List<StateTransition> Transitions = new();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
