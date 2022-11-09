using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject Player;
    public GameObject teleportLocation;
    private bool teleportation = false;
    // Start is called before the first frame update

    private void Update()
    {
        if(teleportation)
        Player.transform.position = teleportLocation.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            teleportation = true;
            Debug.Log("Hit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            teleportation = false;
        }
    }
}
