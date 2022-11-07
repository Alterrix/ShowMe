using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GentleCat.ScriptableObjects.Properties;
using GentleCat.ScriptableObjects.Sets;
using UnityEngine;

public class VolumeDistanceControl : MonoBehaviour
{

    private AudioSource source;
    public TransformVariable player;
    public GameObjectSet monsters;

    public Vector2 volumeRange;
    public Vector2 distanceRange;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    
    // Update is called once per frame
    void Update()
    {
        var closest = monsters.Items.Min(x => Vector3.Distance(x.transform.position, player.CurrentValue.position));
        var volume = Mathf.Clamp(Map(closest, distanceRange.x, distanceRange.y, volumeRange.x, volumeRange.y),
            volumeRange.x, volumeRange.y);
        source.volume = volume;
    }
}
