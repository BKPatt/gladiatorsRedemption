using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public GameObject[] waypoints;
    private int currWaypoint = -1;
    public float lookTime = 0.1f;
    public float minTime = 0f;
    public float maxTime = 3f;
    public Transform AI_Minotaur;
    private NavMeshHit hitInfo;
    public GameObject tracker;

    private enum State
    {
        NotMoving,
        Moving
    }

    private State currState;

    void Start()
    {
        // Initialize components
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found!");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found!");
            return;
        }

        // Initialize variables
        setNextWaypoint();
        currState = State.NotMoving;
        tracker.transform.position = new Vector3(0f, -100f, 0f);
    }

    void Update()
    {
        // Update animation based on speed
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);

        // State machine
        switch (currState)
        {
            case State.NotMoving:
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance == 0)
                {
                    setNextWaypoint();
                }
                break;

            case State.Moving:
                // Movement logic can be inserted here
                break;

            default:
                break;
        }
    }

    private void setNextWaypoint()
    {
        // Loop through waypoints or change state
        if (currWaypoint >= waypoints.Length - 1)
        {
            currWaypoint = -1;
            currState = State.NotMoving;
            tracker.transform.position = new Vector3(0f, -100f, 0f);
        }
        else
        {
            currWaypoint = (currWaypoint + 1) % waypoints.Length;

            if (waypoints.Length == 0)
            {
                Debug.LogWarning("No waypoints set.");
            }
            else
            {
                navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
            }
        }
    }
}
