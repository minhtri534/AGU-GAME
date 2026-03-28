using UnityEngine;

public class TallRatStateMachine : EnemyStateMachine
{
    public override void Start()
    {
        // Add states
        States.Add("Idle", new EnemyIdleState{enemy = Enemy});
        States.Add("Wander", new EnemyWanderState{enemy = Enemy});
        States.Add("Attack", new EnemyAttackState{enemy = Enemy});
        States.Add("Retreat", new EnemyRetreatState(Enemy.statsData.speed * 0.9f){enemy = Enemy});

        ActiveState = States["Idle"];
        Debug.Log(ActiveState);

        // Add parameters
        Parameters.Add("DistanceToPlayer", new StateMachineParameter(ParameterType.Float));

        // Add transitions

        // Idle to Attack
        var tempTransition = new StateTransition
        {
            // Define condition to transition
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat < 9f;
            },
            // Define the state to transition to
            NextState = States["Attack"]
        };
        // Register transition and add to state
        Transitions.Add("IdleToAttack", tempTransition);
        States["Idle"].Transitions.Add(tempTransition);

        // Attack to Retreat
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat < 7f;
            },
            NextState = States["Retreat"]
        };
        Transitions.Add("AttackToRetreat", tempTransition);
        States["Attack"].Transitions.Add(tempTransition);

        // Attack to Wander
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat > 9f;
            },
            NextState = States["Wander"]
        };
        Transitions.Add("AttackToWander", tempTransition);
        States["Attack"].Transitions.Add(tempTransition);

        // Retreat to Wander
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat > 9f;
            },
            NextState = States["Wander"]
        };
        Transitions.Add("RetreatToWander", tempTransition);
        States["Retreat"].Transitions.Add(tempTransition);

        // Wander to Idle
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat > 15f;
            },
            NextState = States["Idle"]
        };
        Transitions.Add("WanderToIdle", tempTransition);
        States["Wander"].Transitions.Add(tempTransition);

        // Wander to Attack
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat < 9f;
            },
            NextState = States["Attack"]
        };
        Transitions.Add("WanderToAttack", tempTransition);
        States["Wander"].Transitions.Add(tempTransition);
    }
}