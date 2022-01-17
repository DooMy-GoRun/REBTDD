// Roman Baranoc 29.12.2021

using UnityEngine;

public class AI_FindTargetState : AI_State
{
    #region VARIABLES
    #endregion

    #region STATES
    public AI_StateId GetId()
    {
        return AI_StateId.FindTarget;
    }

    public void Enter(AI_Agent agent)
    {
    }
    public void Update(AI_Agent agent)
    {
        if (agent.Targeting.HasTarget)
        {
            // If agent has target move to target position
            agent.StateMachine.ChangeState(AI_StateId.MoveToTarget);
        }
        else
        {
            // If there are no targets in sight switch to Idle state
            agent.StateMachine.ChangeState(AI_StateId.Idle);
        }
    }

    public void Exit(AI_Agent agent)
    {
    }
    #endregion
}
