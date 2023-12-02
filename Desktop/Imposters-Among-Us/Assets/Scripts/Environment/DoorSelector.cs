using UnityEngine;

public class DoorSelector : MonoBehaviour
{
    private Camera cam;

    // Define door bounds (replace with your own values)
    private Bounds[] doorBounds = new Bounds[] {
        new Bounds(new Vector3(4.72f, 1.83f, -3.83f), new Vector3(2, 2, 2))  // example bound for Door/Cell #1
    };

    private float doorOpenHeight = 3.0f;  // Define how high the door should move when opened
    private float doorOpenSpeed = 2.0f;   // Define the speed at which the door should open

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Check for a mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with any object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the intersected object is the colosseum
                if (hit.collider.gameObject.name == "SM_Colosseum_01")
                {
                    // Get the point of intersection
                    Vector3 hitPoint = hit.point;

                    // Based on the hitPoint, determine which door or cell was clicked
                    IdentifyDoorOrCell(hitPoint, hit.collider.gameObject);
                }
            }
        }
    }

    void IdentifyDoorOrCell(Vector3 hitPoint, GameObject doorObject)
    {
        // Implement logic to identify which door or cell was clicked based on the hitPoint
        for (int i = 0; i < doorBounds.Length; i++)
        {
            if (doorBounds[i].Contains(hitPoint))
            {
                Debug.Log("Clicked on Door/Cell #" + (i + 1));
                OpenDoor(doorObject);
                break;
            }
        }
    }

    void OpenDoor(GameObject door)
    {
        StartCoroutine(MoveDoorUpwards(door));
    }

    System.Collections.IEnumerator MoveDoorUpwards(GameObject door)
    {
        Vector3 startPos = door.transform.position;
        Vector3 endPos = startPos + new Vector3(0, doorOpenHeight, 0);

        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;

        float distanceCovered = (Time.time - startTime) * doorOpenSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * doorOpenSpeed;
            fractionOfJourney = distanceCovered / journeyLength;

            door.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);

            yield return null;
        }
    }
}
