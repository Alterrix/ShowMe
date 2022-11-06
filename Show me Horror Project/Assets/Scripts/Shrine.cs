using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    public GameObject fire;
    public Lantern lantern;
    public GameObject lanternObj;
    public bool lit = false;
    public bool activateLanternImage = false;

    // Update is called once per frame
    void Update()
    {
        if (lantern.currentTime == 10f)
        {
            activateLanternImage = true;
        }
        else
        {
            activateLanternImage = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.O))
        {
            if (lantern.lanternOn)
            {
                fire.SetActive(true);
                lit = true;
                Debug.Log("Shrine activated");
            }

            if (!lantern.lanternOn && lit)
            {
                lantern.currentTime = 10f;
                lantern.lanternOn = true;
                lanternObj.SetActive(true);
                Debug.Log("Lantern on");
                activateLanternImage = true;
            }
        }
    }
}
