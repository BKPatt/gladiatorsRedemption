using UnityEngine;

public class MakeCubeInvisible : MonoBehaviour
{
    void Start()
    {
        // Fetch the Renderer component from the GameObject this script is attached to
        Renderer rend = GetComponent<Renderer>();

        // Make the GameObject invisible
        if (rend != null)
        {
            rend.enabled = false;
        }
    }
}
