// Roman Baranov 22.12.2021

using UnityEngine;
using UnityEngine.AI;

public class DebugNawMesh : MonoBehaviour
{
    #region VARIABLES
    public bool velocity;
    public bool desiredVelocity;
    public bool path;

    private NavMeshAgent _agent = null;
    #endregion


    #region UNITY Methods
    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        if (velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _agent.velocity);
        }

        if (desiredVelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _agent.desiredVelocity);
        }

        if (path)
        {
            Gizmos.color = Color.black;
            NavMeshPath agentPath = _agent.path;
            Vector3 prevCorner = transform.position;

            foreach (var corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);

                prevCorner = corner;
            }
        }
    }
    #endregion
}
