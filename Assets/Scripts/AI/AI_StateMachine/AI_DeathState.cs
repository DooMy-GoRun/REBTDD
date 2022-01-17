// Roman Baranov 27.12.2021

using UnityEngine;

public class AI_DeathState : AI_State
{
    public AI_StateId GetId()
    {
        return AI_StateId.Death;
    }

    public void Enter(AI_Agent agent)
    {
        Debug.Log($"{agent.gameObject.name} died");
        agent.gameObject.SetActive(false);
    }

    public void Update(AI_Agent agent)
    {

    }

    public void Exit(AI_Agent agent)
    {
        
    }
}
