using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;

    public AudioClip walkingAudio;

    private UnityAction<Vector3> walkingEventListener;

    // Start is called before the first frame update

    private void Awake()
    {
        walkingEventListener = new UnityAction<Vector3>(walkingEventHandler);
    }
    void Start()
    {
        
    }

    private void OnEnable()
    {
        EventManager.StartListening<WalkEvent, Vector3>(walkingEventListener);
    }

    private void OnDisable()
    {
        EventManager.StartListening<WalkEvent, Vector3>(walkingEventListener);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void walkingEventHandler(Vector3 worldPos)
    {

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.walkingAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }
    }
}
