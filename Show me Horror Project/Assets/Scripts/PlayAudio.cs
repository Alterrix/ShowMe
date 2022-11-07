using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayAudio : MonoBehaviour
{
    public AudioClip clip;
    public GameObject audioPlayer;
    public Vector2 pitchRange;

    public void Play()
    {
        var audioObject = Instantiate(audioPlayer,transform.position,quaternion.identity);
        AudioSource audioSource = audioObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.Play();
        Destroy(audioObject,clip.length);
        
    }


}