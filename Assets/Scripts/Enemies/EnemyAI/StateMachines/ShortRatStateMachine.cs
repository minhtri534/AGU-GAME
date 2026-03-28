using UnityEngine;

public class ShortRatStateMachine : EnemyStateMachine
{
    public override void Start()
    {
        // Add states
        States.Add("Chase", new EnemyChaseState{enemy = Enemy});
        States.Add("Wander", new EnemyWanderState{enemy = Enemy});
        States.Add("Attack", new EnemyAttackState{enemy = Enemy});
        States.Add("Retreat", new EnemyRetreatState(Enemy.statsData.speed){enemy = Enemy});

        ActiveState = States["Wander"];
        Debug.Log(ActiveState);

        // Add parameters
        Parameters.Add("DistanceToPlayer", new StateMachineParameter(ParameterType.Float));
        Parameters.Add("TakeDamageTrigger", new StateMachineParameter(ParameterType.Trigger));

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

        // Chase to Retreat
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return false;
            },
            TriggerName = "TakeDamageTrigger",
            NextState = States["Retreat"]
        };
        Transitions.Add("ChaseToRetreat", tempTransition);
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

        // Attack to Retreat
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat <= 3f; 
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
                return Parameters["DistanceToPlayer"].ValueFloat > 11f; 
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
                return Parameters["DistanceToPlayer"].ValueFloat > 13f; 
            },
            NextState = States["Wander"]
        };
        Transitions.Add("RetreatToWander", tempTransition);
        States["Retreat"].Transitions.Add(tempTransition);

        // Retreat to Chase
        tempTransition = new StateTransition
        {
            CheckCondition = () =>
            {
                return Parameters["DistanceToPlayer"].ValueFloat > 9f; 
            },
            NextState = States["Wander"]
        };
        Transitions.Add("RetreatToChase", tempTransition);
        States["Retreat"].Transitions.Add(tempTransition);
    }
}