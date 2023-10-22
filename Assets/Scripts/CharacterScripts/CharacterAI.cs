using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3.0f;
    public float pauseTime = 2.0f;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    private int currentWaypoint = 0;
    private int direction = 1;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 velocity;
    private bool isPaused = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.angularSpeed = 1000;

        if (waypoints.Length < 3)
        {
            Debug.LogError("Please assign at least 3 waypoints.");
        }

        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
    }

    void Update()
    {
        if (isPaused) return;

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);

        if (distanceToWaypoint > 1.0f)
        {
            animator.SetFloat("Speed", 0.3f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            StartCoroutine(Pause());
            UpdateWaypoint();
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }

        velocity.y += gravity * Time.deltaTime;
        navMeshAgent.Move(velocity * Time.deltaTime);
    }

    IEnumerator Pause()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;
    }

    void UpdateWaypoint()
    {
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
}
