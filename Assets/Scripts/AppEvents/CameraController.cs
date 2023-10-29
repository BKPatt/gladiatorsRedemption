using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform desiredPose;
    public Transform target;
    public DialogManager dialogManager; // Reference to the DialogManager
    public float verticalSpeed = 2.0f; // Speed of vertical rotation
    private float pitch = 0.0f; // Vertical angle (pitch)
    public float smoothTime = 0.1f;  // Time to reach the target
    private float velocity = 0.0f;  // Velocity used in SmoothDamp

    void Start()
    {
        if (dialogManager == null)
        {
            dialogManager = FindObjectOfType<DialogManager>();
        }
    }

    void LateUpdate()
    {
        if (desiredPose != null)
        {
            // Directly set the position to match desiredPose
            transform.position = desiredPose.position;
            // Horizontal rotation
            transform.rotation = Quaternion.LookRotation(desiredPose.forward, Vector3.up);

            if (!dialogManager.isInDialogue)
            {
                // Vertical rotation (pitch) when not in dialogue
                pitch -= verticalSpeed * Input.GetAxis("Mouse Y");
                pitch = Mathf.Clamp(pitch, -45f, 45f);
                transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0);
            }
            else
            {
                // Make camera look towards NPC if in dialogue
                GameObject npc = GameObject.Find(dialogManager.currentNPC);
                if (npc != null)
                {
                    Vector3 toNpc = npc.transform.position - transform.position;
                    float targetPitch = Mathf.Atan2(toNpc.y, toNpc.magnitude) * Mathf.Rad2Deg;

                    // Smoothly interpolate between current and target pitch
                    pitch = Mathf.SmoothDamp(pitch, targetPitch, ref velocity, smoothTime);

                    pitch = Mathf.Clamp(pitch, -45f, 45f);
                    transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0);
                }
            }
        }
    }
}
