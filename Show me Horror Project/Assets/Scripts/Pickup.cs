using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    public LanternVariable lantern;

    public UnityEvent onPickup;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onPickup.Invoke();

            Destroy(gameObject);
        }
    }

    public void TimerUpgrade()
    {
        //lantern.CurrentValue.currentTime = 30f;
        lantern.CurrentValue.maxTime = 20f;
    }

    public void RangeUpgrade()
    {
        lantern.CurrentValue.pickedUpLanternLightUpgrade = true;
    }
}
