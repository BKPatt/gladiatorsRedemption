using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    public Transform[] waypoints;
    private Transform[] originalWaypoints;
    public float pauseTime = 2.0f;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;

    private int currentWaypoint = 0;
    private int direction = 1;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isPaused = false;
    public bool isDialoguePaused = false;
    public Transform playerTransform;

    void Start()
    {
        originalWaypoints = waypoints;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned. Character will not move.");
            return;
        }

        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
    }

    void Update()
    {
        if (isPaused || isDialoguePaused)
        {
            navMeshAgent.isStopped = true;
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);

            // Rotate to face the player during dialogue
            if (isDialoguePaused && playerTransform != null)
            {
                FaceTarget(playerTransform);
            }

            return;
        }

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned. Character will not move.");
            return;
        }

        navMeshAgent.isStopped = false;

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);

        if (distanceToWaypoint > 1.0f)
        {
            animator.SetFloat("Speed", 0.3f, 0.1f, Time.deltaTime);
        }
        else
        {
            StartCoroutine(Pause());
        }
    }

    IEnumerator Pause()
    {
        isPaused = true;
        navMeshAgent.isStopped = true;
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;

        if (waypoints != null && waypoints.Length > 0)
        {
            UpdateWaypoint();
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    public void SetDestination(Transform newDestination)
    {
        if (newDestination != null)
        {
            waypoints = new Transform[] { newDestination };  // Set only the new destination
            navMeshAgent.SetDestination(newDestination.position);
            isDialoguePaused = true;  // Pause AI movement for dialogue
        }
        else
        {
            waypoints = originalWaypoints;  // Restore original waypoints
            UpdateWaypoint();  // Update the waypoint index
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
            isDialoguePaused = false;  // Resume AI movement
        }
    }

    void UpdateWaypoint()
    {
        if (waypoints == null || waypoints.Length <= 1)
        {
            Debug.LogWarning("Not enough waypoints for a meaningful path.");
            return;
        }

        if (currentWaypoint == 0)
        {
            direction = 1;
        }
        else if (currentWaypoint == waypoints.Length - 1)
        {
            direction = -1;
        }
        currentWaypoint += direction;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
