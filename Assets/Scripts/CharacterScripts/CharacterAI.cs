using System.Collections;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3.0f;
    public float pauseTime = 2.0f;

    private int currentWaypoint = 0;
    private int direction = 1;
    private CharacterController characterController;
    private Animator animator;
    private bool isPaused = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (waypoints.Length < 3)
        {
            Debug.LogError("Please assign at least 3 waypoints.");
        }
    }

    void Update()
    {
        if (isPaused) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 moveDirection = (target.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, target.position) > 0.5f)
        {
            characterController.SimpleMove(moveDirection * moveSpeed);
            animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            if (currentWaypoint == 0 || currentWaypoint == waypoints.Length - 1)
            {
                StartCoroutine(Pause());
            }
            UpdateWaypoint();
        }
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
