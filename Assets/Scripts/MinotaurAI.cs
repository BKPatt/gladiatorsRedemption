using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurAI : MonoBehaviour
{
    // Require and reference component UnityEngine.AI.NavMeshAgent
    // Grab a reference to the Minion’s Animator as well
    // Add a public array of GameObjects to MinionAI named waypoints.
    // Add an int currWaypoint property

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    public GameObject[] waypoints;
    public int currWaypoint;
    public float lookTime = 0.1f;
    public float minTime = 0f;
    public float maxTime = 3f;
    //public Transform Cube_Moving;
    public Transform AI_Minotaur;
    //private Vector3 cubePos;
    //private Vector3 cubeVel;
    private NavMeshHit hitInfo;
    public GameObject tracker;

    private enum state
    {
        NotMoving,
        Moving
    }

    private state currState;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        currWaypoint = -1;
        setNextWaypoint();
        currState = state.NotMoving;
        tracker.transform.position = new Vector3(0f, -100f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
        switch(currState)
        {
            case state.NotMoving:
            
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance == 0)
                {
                    setNextWaypoint();
                }
                break;

            case state.Moving:

                // Track down
                /*cubePos = Cube_Moving.position;
                VelocityReporter velocityReporter = Cube_Moving.GetComponent<VelocityReporter>();
                cubeVel = velocityReporter.velocity;
                float disToCube = Vector3.Distance(transform.position, cubePos);
                lookTime = disToCube / cubeVel.magnitude;
                lookTime = Mathf.Clamp(lookTime, minTime, maxTime);
                Vector3 predictedPos = waypoints[currWaypoint].transform.position + cubeVel*lookTime;
                //Debug.Log("Predicted Pos: " + predictedPos);
                navMeshAgent.SetDestination(predictedPos);
                tracker.SetActive(true);
                tracker.transform.position = predictedPos;
                //tracker.transform.position = new Vector3(0f, 0f, 0f);
                
                if (NavMesh.Raycast(AI_Minion.position, predictedPos, out hitInfo, NavMesh.AllAreas))
                {
                    Debug.Log("Obstruction detected: ");
                }
                if (!navMeshAgent.pathPending && Mathf.Abs((AI_Minion.position - Cube_Moving.position).magnitude) <= 0.5)
                {
                    setNextWaypoint();
                }*/

                break;

            
            default:
                break;
        }        
    }

    

    private void setNextWaypoint()
    {
        if (currWaypoint >= 5)
        {
            currWaypoint = -1;
            currState = state.NotMoving;
            tracker.transform.position = new Vector3(0f, -100f, 0f);
        }
        else
        {
            if (currWaypoint >= 12) //4
            {
                currState = state.Moving;
            }

            currWaypoint = (currWaypoint + 1) % waypoints.Length;
            //Debug.Log("Current waypoint: " + currWaypoint);
            if (waypoints.Length == 0)
            {
                Debug.LogWarning("error due to waypoints array of length 0");
            }
            else
            {
                navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
            }
        }

        
    }
}