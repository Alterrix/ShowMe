using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    public Lantern lantern;
    public AudioSource pickupSound;
    public GameObject TimerText;
    public GameObject RangeText;
    // Start is called before the first frame update

    public void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (tag)
            {
                case "Timer":
                    TimerUpgrade();
                    break;
                case "RangeUpgrade":
                    RangeUpgrade();
                    break;
                default:
                    break;
            }
            pickupSound.Play();
            
            Destroy(gameObject,1f);
        }
        //lanternrange upgrade
    }

    private void TimerUpgrade()
    {
        TimerText.SetActive(true);
        lantern.currentTime = 30f;
        lantern.maxTime = 30f;
        Debug.Log("Picked up time upgrade");
        
    }

    private void RangeUpgrade()
    {
        lantern.pickedUpLanternLightUpgrade = true;
        RangeText.SetActive(true);
    }
}
