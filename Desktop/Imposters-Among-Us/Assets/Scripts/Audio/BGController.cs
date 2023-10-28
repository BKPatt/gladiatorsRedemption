using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundState
{
    Play,
    Stop
}

public class BGController : MonoBehaviour
{
    // Start is called before the first frame update

    public SoundState ShouldPlayBG = SoundState.Play;

    private void Awake()
    {
        if(ShouldPlayBG == SoundState.Stop)
        {
            var a = GetComponent<AudioSource>();
            a.Stop();
        }
    }
}
