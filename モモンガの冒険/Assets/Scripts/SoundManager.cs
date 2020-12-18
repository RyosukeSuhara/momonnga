using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    AudioSource audio;

    void Start()
    {
        this.audio = GetComponent<AudioSource>();
        this.audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
