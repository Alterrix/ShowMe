using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lantern;
    public bool lanternOn = true;
    public float currentTime;
    public AIEnemy enemy;
    public float startingTime = 10f;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("pressed");
            lantern.SetActive(!lanternOn);
            lanternOn = !lanternOn;
            currentTime = startingTime;
        }

        if (currentTime <= 0)
        {
            lantern.SetActive(false);
            lanternOn = false;
            enemy.speedRun = 9f;
        }
    }
}
