using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIMovement : MonoBehaviour
{
    // Private Serialized Fields
    [SerializeField] private Transform player;
    [SerializeField] private float attackDist = 15.0f;
    [SerializeField] private float chaseDist = 20.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private DialogManager dialogManager;

    public PlayerMovement PlayerMovement { get; private set; }
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isMoving;
    private bool inRange;
    public bool inAttackAI { get; private set; }
    private bool turnAgain;
    private bool chase;

    private float timePassed;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        PlayerMovement = player.GetComponent<PlayerMovement>();
        ResetState();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        timePassed += Time.deltaTime;

        if (ShouldMove(distance))
        {
            MoveTowardsPlayer();
        }
        else if (distance < chaseDist)
        {
            chase = true;
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
        return (distance < attackDist || chase) && gameObject.GetComponent<NavMeshAgent>().enabled;
    }

    private void MoveTowardsPlayer()
    {
        if (navMeshAgent == null || !navMeshAgent.enabled || !navMeshAgent.isOnNavMesh || dialogManager.isInDialogue)
        {
            return;
        }

        navMeshAgent.SetDestination(player.position);
        isMoving = true;
        chase = true;
    }


    private void StopAndRotateTowardsPlayer()
    {
        if (isMoving)
        {
            isMoving = false;
            turnAgain = true;
        }

        Vector3 lookDirection = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
    }

    private bool ShouldAttack(float distance)
    {
        var isClose = distance < 3.0f;
        if (isClose && (timePassed > 1f))
        {
            timePassed = 0f;
            return true;
        }
        return false;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
            turnAgain = false;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Speed", Mathf.Min(navMeshAgent.velocity.magnitude / navMeshAgent.speed, 0.5f));
    }

    private IEnumerator Attack()
    {
        inAttackAI = true;
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Idle");
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        yield return new WaitForSeconds(1.5f);

        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        yield return new WaitForSeconds(1.0f);

        gameObject.GetComponent<NavMeshAgent>().enabled = true;

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
