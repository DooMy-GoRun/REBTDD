// Roman Baranov 04.01.2022

using UnityEngine;

public class AI_MoveToTargetState : AI_State
{
    #region VARIABLES
    private Vector3 _destination;

    // TO DO
    private float _attackRange = 1.5f;
    #endregion

    #region STATES
    public AI_StateId GetId()
    {
        return AI_StateId.MoveToTarget;
    }

    public void Enter(AI_Agent agent)
    {
        Debug.Log("AI_MoveToTargetState.Enter");
        _destination = agent.NavMeshAgent.destination;
    }

    public void Update(AI_Agent agent)
    {
        if (!agent.Targeting.HasTarget)
        {
            agent.StateMachine.ChangeState(AI_StateId.FindTarget);
        }
        else
        {
            // Update destination if the target moves one unit
            if (Vector3.Distance(_destination, agent.Targeting.TargetPosition) > 0.1f)
            {
                _destination = agent.Targeting.TargetPosition;
                agent.NavMeshAgent.destination = _destination;
            }
        }

        // Check if agent reached its target
        float destination = Vector3.Distance(agent.gameObject.transform.position, agent.Targeting.TargetPosition);

        // and target is inside agent attackRange
        if (destination <= _attackRange)
        {
            agent.StateMachine.ChangeState(AI_StateId.AttackTarget);
            return;
        }
    }

    public void Exit(AI_Agent agent)
    {
    }
    #endregion


}
