using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIMovement : MonoBehaviour
{
    // Private Serialized Fields
    [SerializeField] private Transform player;
    [SerializeField] private float attackDist = 15.0f;
    [SerializeField] private int damage = 10;

    // Public and Private Fields
    public PlayerMovement PlayerMovement { get; private set; }
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isMoving;
    private bool inRange;
    public bool inAttackAI { get; private set; }
    private bool turnAgain;
    private bool chase;

    // Initialize the component
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        PlayerMovement = player.GetComponent<PlayerMovement>();
        ResetState();
    }

    // Update the component
    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (ShouldMove(distance))
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopAndRotateTowardsPlayer();
        }

        if (ShouldAttack(distance))
        {
            StartAttack();
        }

        UpdateAnimator();
    }

    private void ResetState()
    {
        isMoving = false;
        inRange = false;
        inAttackAI = false;
        turnAgain = true;
        chase = false;
    }

    private bool ShouldMove(float distance)
    {
        return distance < attackDist || chase;
    }

    private void MoveTowardsPlayer()
    {
        navMeshAgent.SetDestination(player.position);
        isMoving = true;
        chase = true;
    }

    private void StopAndRotateTowardsPlayer()
    {
        if (isMoving)
        {
            navMeshAgent.isStopped = true;
            isMoving = false;
            turnAgain = true;
        }

        Vector3 lookDirection = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    private bool ShouldAttack(float distance)
    {
        return distance < 3.0f;
    }

    private void StartAttack()
    {
        inRange = true;
        inAttackAI = true;
        StartCoroutine(Attack());

        if (turnAgain)
        {
            Vector3 lookDirection = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
            turnAgain = false;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
    }

    private IEnumerator Attack()
    {
        inAttackAI = true;
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(3.0f);

        animator.SetTrigger("Idle");
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        yield return new WaitForSeconds(3.0f);

        inAttackAI = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAxe") && PlayerMovement.inAttackPlayer)
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Hit by player!");
            }
        }
    }
}
