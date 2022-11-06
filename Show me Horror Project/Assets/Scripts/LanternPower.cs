using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanternPower : MonoBehaviour
{
    public Image LanternUI;
    [SerializeField] float DrainTime = 10;
    public Lantern lantern;
    public Shrine shrine;

    // Update is called once per frame
    void Update()
    {
        if (lantern.lanternOn)
        {
            LanternUI.fillAmount -= 1.0f / DrainTime * Time.deltaTime;
        }
    }

    public void RefillImage()
    {
        LanternUI.fillAmount = 1;
    }
}