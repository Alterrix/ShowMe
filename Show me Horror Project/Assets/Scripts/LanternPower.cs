using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using UnityEngine;
using UnityEngine.UI;

public class LanternPower : MonoBehaviour
{
    public Image LanternUI;
    public LanternVariable lantern;
    public Shrine shrine;

    // Update is called once per frame
    void Update()
    {
        LanternUI.fillAmount = lantern.CurrentValue.currentTime / lantern.CurrentValue.startingTime;
    }
}