// Roman Baranov 24.12.2021

#region ENUM
/// <summary>
/// Available states
/// </summary>
public enum AI_StateId
{
    MoveToTarget,
    FindTarget,
    AttackTarget,
    Death,
    Idle
}
#endregion

#region INTERFACE
public interface AI_State
{
    /// <summary>
    /// Get current state Id
    /// </summary>
    /// <returns>Return current state</returns>
    public AI_StateId GetId();

    /// <summary>
    /// New state to enter
    /// </summary>
    /// <param name="agent"></param>
    public void Enter(AI_Agent agent);

    /// <summary>
    /// Update current state
    /// </summary>
    /// <param name="agent"></param>
    public void Update(AI_Agent agent);

    /// <summary>
    /// Exit current state
    /// </summary>
    /// <param name="agent"></param>
    public void Exit(AI_Agent agent);
}
#endregion
