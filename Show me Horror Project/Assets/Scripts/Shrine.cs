using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Shrine : MonoBehaviour
{
    public GameObject fire;
    public Lantern lantern;
    public bool lit = false;
    public GameObject lanternObj;

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

            if (!lantern.lanternOn && lit)
            {
                lantern.currentTime = 10f;
                lantern.lanternOn = true;
                lanternObj.SetActive(true);
                Debug.Log("Lantern on");
            }
        }
    }
}
