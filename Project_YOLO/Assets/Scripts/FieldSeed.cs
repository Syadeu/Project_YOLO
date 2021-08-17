using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSeed : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (PlayerController.instance.haveSeed) return;
        
        PlayerController.instance.SeedAcquisition();
        Destroy(gameObject);
    }
}
