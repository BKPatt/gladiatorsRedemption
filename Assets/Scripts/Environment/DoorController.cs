using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator component is missing on the door object.");
        }
    }

    // Call this method when player interacts with the door
    public void Interact()
    {
        if (_isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _isOpen = true;
        _animator.SetTrigger("OpenDoor");
    }

    private void CloseDoor()
    {
        _isOpen = false;
        _animator.SetTrigger("CloseDoor");
    }
}
