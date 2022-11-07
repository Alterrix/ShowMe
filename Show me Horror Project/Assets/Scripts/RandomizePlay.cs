using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizePlay : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource source;
    public Vector2 pitchRange;
    public float playChance = 0.1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        if (source.isPlaying)
            return;
        if (Random.Range(0.0f, 1.0f) < playChance)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.pitch = Random.Range(pitchRange.x, pitchRange.y);
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
