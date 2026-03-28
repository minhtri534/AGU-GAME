using UnityEngine;

public class FatRatStateMachine : EnemyStateMachine
{
    public override void Start()
    {
        // Add states
        States.Add("Chase", new EnemySmartChaseState(6f){enemy = Enemy});
        States.Add("Wander", new EnemyWanderState{enemy = Enemy});
        States.Add("Attack", new EnemyAttackState{enemy = Enemy});

        ActiveState = States["Wander"];

        // Add parameters
        Parameters.Add("DistanceToPlayer", new StateMachineParameter(ParameterType.Float));

        // Add transitions

        // Wander to Chase
        var tempTransition = new StateTransition
        {
            // Define condition to transition
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat < 11f;
            },
            // Define the state to transition to
            NextState = States["Chase"]
        };
        // Register transition and add to state
        Transitions.Add("WanderToChase", tempTransition);
        States["Wander"].Transitions.Add(tempTransition);

        // Chase to Attack
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat < 3f;
            },
            NextState = States["Attack"]
        };
        Transitions.Add("ChaseToAttack", tempTransition);
        States["Chase"].Transitions.Add(tempTransition);

        // Chase to Wander
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat > 11f;
            },
            NextState = States["Wander"]
        };
        Transitions.Add("ChaseToWander", tempTransition);
        States["Chase"].Transitions.Add(tempTransition);

        // Attack to Chase
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat > 3f;
            },
            NextState = States["Chase"]
        };
        Transitions.Add("AttackToChase", tempTransition);
        States["Attack"].Transitions.Add(tempTransition);
    }
}