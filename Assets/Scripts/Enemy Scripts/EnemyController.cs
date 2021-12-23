using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Behaviour[] components;
    [SerializeField] private LayerMask layers;
    public Transform player;
    private Vector3 startPosition;
    private Vector3 finishPosition;

    private NavMeshAgent navMeshAgent;
    private Vector3 currentPosition;

    [SerializeField] private Animator animator;
    [SerializeField] private string walkParameterFloat;

    private Vector3 oldPosition;

    private float current_Attack_time;
    [SerializeField] private float default_Attack_time = 0.3f;

    private bool _lowhp;   


    // Start is called before the first frame update
    void Start()
    {
        _lowhp = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentPosition = startPosition;
        navMeshAgent.SetDestination(currentPosition);

        current_Attack_time = default_Attack_time;
    }

    // Update is called once per frame
    void Update()
    {
        OnLift();
        FindTarget();
        CheckWalk();
    }

    private void FindTarget()
    {
        if (player != null && gameObject.layer != LayerMask.NameToLayer("Death") && gameObject.layer != LayerMask.NameToLayer("DownLayer"))
        {
            navMeshAgent.SetDestination(player.position);

            foreach (var component in components)
                component.enabled = true;
        }

        if (Vector3.Distance(transform.position, currentPosition) > 1)
        {   
            return;
        }

        if (currentPosition == startPosition)
            currentPosition = finishPosition;
        else
            currentPosition = startPosition;
        
        navMeshAgent.SetDestination(currentPosition);
    }

    public void SetupPositionToStart()
    {
        currentPosition = startPosition;
    }

    public void OnEnable()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.SetDestination(currentPosition);

            foreach (var component in components)
                component.enabled = true;
        }

        //private void OnEnable() => navMeshAgent?.SetDestination(currentPosition);
    }

    private void CheckWalk()
    {
        var speed = Vector3.Distance(transform.position, oldPosition);
        speed = Mathf.Abs(speed);
        oldPosition = transform.position;

        //rotate enemy 2.5D
        if (navMeshAgent.velocity.x > 0)
            transform.rotation = Quaternion.Euler(0, 90f, 0);
        else if (navMeshAgent.velocity.x < 0)
            transform.rotation = Quaternion.Euler(0, -90f, 0);

        if((navMeshAgent.velocity.x > 0 && navMeshAgent.velocity.y > 0) || (navMeshAgent.velocity.x > 0 && navMeshAgent.velocity.y < 0))
            transform.rotation = Quaternion.Euler(0, 90f, 0);
        else if ((navMeshAgent.velocity.x < 0 && navMeshAgent.velocity.y > 0) || (navMeshAgent.velocity.x < 0 && navMeshAgent.velocity.y < 0))
            transform.rotation = Quaternion.Euler(0, -90f, 0);

        animator.SetFloat(walkParameterFloat, speed);

        //need set to back move for agent
        if(navMeshAgent.velocity.x > 0 && Vector3.Distance(transform.position, currentPosition) < 3)
        {
            //currentPosition = transform.position + Vector3.forward;
        }
        if(navMeshAgent.velocity.x < 0 && Vector3.Distance(transform.position, currentPosition) < 3)
        {
            //currentPosition = transform.position + Vector3.back;
        }

        //check to attack
        if (speed < 0.000001)
        {
            Attacks();
        }
    }

    private void Attacks()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, transform.forward * 3f, Color.yellow);
        RaycastHit hit;

        animator.SetBool("BackMovement", false);
        current_Attack_time += Time.deltaTime;

        if (current_Attack_time > default_Attack_time)
        {
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.distance < 0.5)
                    EnemyAttack(0);

                if (hit.distance > 0.5 && hit.distance < 1.5)
                    EnemyAttack(1);

                if (hit.distance > 2.5)
                    EnemyAttack(2);
            }
        }
    }


    private void EnemyAttack(int attack)
    {
        if (attack == 0)
        {
            //navMeshAgent.isStopped = true;
            animator.SetTrigger(AnimationTags.ATTACK_1_TRIGGER);
        }

        if (attack == 1)
        {
            //navMeshAgent.isStopped = true;
            animator.SetTrigger(AnimationTags.ATTACK_2_TRIGGER);
        }

        if (attack == 2)
        {
            animator.SetBool("BackMovement", true);
            //navMeshAgent.isStopped = true;
            animator.SetTrigger(AnimationTags.ATTACK_3_TRIGGER);
        }

        //reset time to attack
        current_Attack_time = 0f;

    }

    private void OnLift()
    {
        if(gameObject.transform.parent != null)
        {
            animator.SetBool("onLift", true);
        }
        if (gameObject.transform.parent == null)
            animator.SetBool("onLift", false);
            
    }

    //for Animation Events

    private void ReallyDeath()
    {
        foreach (var component in components)
            component.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        
        Destroy(gameObject, 7f);
     
        
    }

    private void AgentStop()
    {
        foreach (var component in components)
            component.enabled = false;
        if (gameObject.layer == LayerMask.NameToLayer("LayerToSuper"))
            _lowhp = true;

            gameObject.layer = LayerMask.NameToLayer("DownLayer");
    }

    private void AgentResume()
    {
        foreach (var component in components)
            component.enabled = true;

        if(_lowhp || gameObject.layer == LayerMask.NameToLayer("LayerToSuper"))
            gameObject.layer = LayerMask.NameToLayer("LayerToSuper");
        else
            gameObject.layer = LayerMask.NameToLayer("Enemy");

        gameObject.transform.parent = null;
    }

    private void StopNav()
    {
        navMeshAgent.isStopped = true;
    }
    private void ResumeNav()
    {
        navMeshAgent.isStopped = false;
    }
}
