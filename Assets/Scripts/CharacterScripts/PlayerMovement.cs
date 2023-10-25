using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed = 700.0f;
    public float proximityRadius = 1.5f;
    public LayerMask detectableObjects;
    public GameObject interactUI;
    public GameObject currentInterlocutor;

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

    private bool isFirstGlimpseTriggered = false;
    private bool draxusDialogueStarted = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (dialogManager.isInDialogue)
        {
            Idle();
            FaceInterlocutor(GameObject.Find(dialogManager.currentNPC));

            GameObject npc = GameObject.Find(dialogManager.currentNPC);
            if (npc != null)
            {
                CharacterAI npcAI = npc.GetComponent<CharacterAI>();
                if (npcAI != null)
                {
                    if (dialogManager.isInDialogue)
                    {
                        npcAI.SetDestination(transform);
                        FaceInterlocutor(npc);
                    }
                    else
                    {
                        npcAI.SetDestination(null);
                    }
                }
            }

            return;
        }

        Move();
        CheckProximity();
        CheckForInteractions();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
    }

    private void FaceInterlocutor(GameObject interlocutor)
    {
        if (interlocutor == null)
            return;

        Vector3 directionToInterlocutor = (interlocutor.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToInterlocutor.x, 0, directionToInterlocutor.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }


    private void CheckProximity()
    {
        bool interactableInRange = false;
        float draxusRange = 4.0f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityRadius, detectableObjects);

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"PlayerMovement: Detected object: {hitCollider.gameObject.name}");

            if (hitCollider.CompareTag("Draxus") && !draxusDialogueStarted && Vector3.Distance(transform.position, hitCollider.transform.position) <= draxusRange)
            {
                if (dialogManager.currentSceneIndex == 0)
                {
                    dialogManager.StartDialogue("Draxus");
                    draxusDialogueStarted = true;
                }
                continue;
            }

            if (hitCollider.CompareTag("NPC") || hitCollider.GetComponent<DoorwayToTrainingRoom>() != null || hitCollider.GetComponent<DoorController>() != null)
            {
                interactableInRange = true;
                break;
            }
        }

        interactUI.SetActive(interactableInRange);
        if (interactableInRange)
        {
            interactUI.transform.parent.gameObject.SetActive(true);
            Text uiText = interactUI.GetComponentInChildren<Text>(true);
            if (uiText != null)
            {
                uiText.text = "Press E to Interact";
                uiText.gameObject.SetActive(true);
            }
        }
        else
        {
            interactUI.transform.parent.gameObject.SetActive(false);
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
        EventManager.TriggerEvent<WalkEvent, Vector3>(new Vector3());
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
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (!isFirstGlimpseTriggered)
        {
            if (currentSceneName == "Cell")
            {
                isFirstGlimpseTriggered = true;
                dialogManager.StartDialogue("FirstGlimpse");
            }
            else if (currentSceneName == "TrainingRoom")
            {
                isFirstGlimpseTriggered = true;
                dialogManager.StartDialogue("Caelia");
            }
        }

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

                if (hitCollider.CompareTag("MoveScene"))
                {
                    if (currentSceneName == "Cell")
                    {
                        SceneManager.LoadScene("TrainingRoom");
                    }
                    else if (currentSceneName == "TrainingRoom")
                    {
                        SceneManager.LoadScene("Colosseum");
                    }
                    return;
                }

                if (hitCollider.CompareTag("NPC"))
                {
                    string characterName = hitCollider.gameObject.name;
                    dialogManager.StartDialogue(characterName);
                }
            }
        }
    }
}
