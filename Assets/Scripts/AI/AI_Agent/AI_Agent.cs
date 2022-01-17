// Roman Baranov 25.12.2021

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AI_TargetingSystem))]
public class AI_Agent : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private AI_StateId _initialState;

    private AI_StateMachine _stateMachine = null;
    public AI_StateMachine StateMachine { get { return _stateMachine; } }

    private NavMeshAgent _navMeshAgent = null;
    /// <summary>
    /// NavMeshAgent component of the game object
    /// </summary>
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }

    private AI_TargetingSystem _targeting = null;
    public AI_TargetingSystem Targeting { get { return _targeting; } }
    #endregion

    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get components referrences
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _targeting = GetComponent<AI_TargetingSystem>();

        // State machine
        _stateMachine = new AI_StateMachine(this);
        _stateMachine.RegisterState(new AI_MoveToTargetState());
        _stateMachine.RegisterState(new AI_AttackTargetState());
        _stateMachine.RegisterState(new AI_DeathState());
        _stateMachine.RegisterState(new AI_IdleState());
        _stateMachine.RegisterState(new AI_FindTargetState());

        _stateMachine.ChangeState(_initialState);
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();
    }
    #endregion
}
