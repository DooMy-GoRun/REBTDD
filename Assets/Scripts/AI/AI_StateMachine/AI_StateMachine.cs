// Roman Baranov 25.12.2021

public class AI_StateMachine
{
    #region VARIABLES
    public AI_State[] states = null;
    public AI_Agent agent = null;
    public AI_StateId currentState;
    #endregion

    #region CONSTRUCTOR
    public AI_StateMachine(AI_Agent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AI_StateId)).Length;

        states = new AI_State[numStates];
    }
    #endregion

    #region PUBLIC Methods
    /// <summary>
    /// Adds a new state to AI agent state machine
    /// </summary>
    /// <param name="state">AI State to add</param>
    public void RegisterState(AI_State state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    /// <summary>
    /// Gets current AI_State interface of the AI agent
    /// </summary>
    /// <param name="stateId">AI state Id to search for</param>
    /// <returns>Required AI state</returns>
    public AI_State GetState(AI_StateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    /// <summary>
    /// Updates current agent state
    /// </summary>
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    /// <summary>
    /// Changes current agent state to the new one. Calls Exit() of the current state, then Enter() of the new state 
    /// </summary>
    /// <param name="newState">New state to switch to</param>
    public void ChangeState(AI_StateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
    #endregion
}
