// Roman Baranov 27.12.2021

using UnityEngine;

public class AI_IdleState : AI_State
{
    #region VARIABLES
    private float _maxIdleDelay = 3f;
    private float _idleDelay = 1f;// Delay after switching to idle state
    #endregion

    #region STATES
    // TO DO 
    // Make generic state
    public AI_StateId GetId()
    {
        return AI_StateId.Idle;
    }

    public void Enter(AI_Agent agent)
    {
        Debug.Log("AI_IdleState.Enter");
        _idleDelay = Random.Range(0.5f, _maxIdleDelay);
    }

    public void Update(AI_Agent agent)
    {
        // Check idle delay before change to new state
        _idleDelay += Time.deltaTime;
        if (_idleDelay >= _maxIdleDelay)
        {
            agent.StateMachine.ChangeState(AI_StateId.FindTarget);
        }
    }

    public void Exit(AI_Agent agent)
    {
    }
    #endregion
}
