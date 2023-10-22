using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed = 700.0f;
    public float proximityRadius = 2.0f;
    public LayerMask detectableObjects;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private CharacterController controller;
    private Animator animator;
    public DialogManager dialogManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        CheckProximity();
        CheckForInteractions();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
    }

    private void CheckProximity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityRadius, detectableObjects);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("Detected object: " + hitCollider.gameObject.name);
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float turn = Input.GetAxis("Mouse X");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);

        float strafe = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(strafe, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private IEnumerator Attack()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
    }

    private void CheckForInteractions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityRadius, detectableObjects);
        foreach (var hitCollider in hitColliders)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DoorController door = hitCollider.GetComponent<DoorController>();
                if (door != null)
                {
                    door.Interact();
                }

                // Check if the GameObject has an "NPC" tag.
                if (hitCollider.CompareTag("NPC"))
                {
                    // The characterName could be derived from the GameObject's name, 
                    // or you could use a separate script to hold this data.
                    string characterName = hitCollider.gameObject.name;

                    // Trigger the dialogue based on the character name.
                    dialogManager.StartNPCDialogue(characterName);
                }
            }
        }
    }
}
