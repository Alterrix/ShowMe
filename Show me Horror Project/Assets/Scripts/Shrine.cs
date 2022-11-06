using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    public GameObject fire;
    public GameObject lanternObj;
    public Lantern lantern;
    public LanternPower lanternPower;
    public bool lit = false;

    // Update is called once per frame
    void Update()
    {
        
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

            if (lit)
            {
                lantern.currentTime = 10f;
                lantern.lanternOn = true;
                lanternObj.SetActive(true);
                Debug.Log("Lantern on");
                lanternPower.RefillImage();
            }
        }
    }
}
