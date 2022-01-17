// Roman Baranov 29.12.2021

using UnityEngine;

public class AI_AttackTargetState : AI_State
{
    #region VARIABLES
    private float _attackDistance = 1.5f;
    #endregion

    #region STATES
    public AI_StateId GetId()
    {
        return AI_StateId.AttackTarget;
    }

    public void Enter(AI_Agent agent)
    {
        Debug.Log("AI_AttackTargetState.Enter");
    }

    public void Update(AI_Agent agent)
    {
        if (agent.Targeting.HasTarget && agent.Targeting.Target)
        {
            //Check if agent is facing the target
            // Update target position
            Vector3 targetPosition = agent.Targeting.Target.transform.position;
            // Aim agent at target
            AimAtTarget(agent.transform, targetPosition);

            //if target in attack range
            if (Vector3.Distance(agent.transform.position, targetPosition)<= _attackDistance)
            {
                // Attack agent target with attack speed delay
                // Damage target;
                Debug.Log($"Target {agent.Targeting.Target.name} attacked");
            }
            else
            {
                agent.StateMachine.ChangeState(AI_StateId.MoveToTarget);
                return;
            }
        }
        else
        {
            // if trget not exist switch to Find target state
            agent.StateMachine.ChangeState(AI_StateId.FindTarget);
            return;
        }
    }

    public void Exit(AI_Agent agent)
    {
    }
    #endregion

    #region PRIVATE Methods
    /// <summary>
    /// Turn agent to target 
    /// </summary>
    /// <param name="agent">This agent</param>
    /// <param name="targetPosition">Target to turn to</param>
    private void AimAtTarget(Transform agent, Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - agent.transform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(agent.transform.forward, targetDirection);
        agent.rotation = aimTowards * agent.rotation;
    }
    #endregion



}
