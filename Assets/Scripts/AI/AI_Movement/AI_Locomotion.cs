// Roman Baranov 22.12.2021

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AI_Locomotion : MonoBehaviour
{
    #region VARIABLES
    private NavMeshAgent _agent = null;
    private Animator _animator = null;
    #endregion

    #region UNITY Methods
    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
    #endregion
}
