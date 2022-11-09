using System;
using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using GentleCat.ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.AI;

public class Shrine : MonoBehaviour
{
    public TransformVariable playerTransform;
    public GameObject fire;
    public LanternVariable lantern;
    public float range = 7.5f;
    public float useRange = 2f;
    public bool lit = false;
    public ShrineSet shrines;
    private NavMeshObstacle obstacle;
    public GameObject shrineMiniMap;


    private void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        if(lit)
            shrines.Add(this);
    }

    private void OnDisable()
    {
        if (shrines.Items.Contains(this))
            shrines.Remove(this);
    }

    private void Update()
    {
        if (lit && obstacle.radius < range)
        {
            obstacle.radius += Time.deltaTime*4;
        }
        if (Input.GetKeyDown(KeyCode.E) &&
            Vector3.Distance(playerTransform.CurrentValue.position, transform.position) < useRange)
        {
            if (lantern.CurrentValue.LanternOn && !lit)
            {
                fire.SetActive(true);
                lit = true;
                shrines.Add(this);
                shrineMiniMap.SetActive(true);
            }

            if (lit)
            {
                lantern.CurrentValue.currentTime = lantern.CurrentValue.maxTime;
            }
        }
    }


    public bool IsInside(Vector3 point)
    {
        return Vector3.Distance(point, transform.position) <= range;
    }
}