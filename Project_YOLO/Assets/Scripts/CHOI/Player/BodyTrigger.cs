using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
    {
        if (!other.gameObject.CompareTag("PassFloor")) return;
        if (other.isTrigger) return;
        if (PlayerController.instance.onPassFloors.Contains(other)) return;
        
        PlayerController.instance.onPassFloors.Add(other);
    }
    
    private void OnTriggerExit (Collider other)
    {
        if (!other.gameObject.CompareTag("PassFloor")) return;
        if (other.isTrigger) return;
        
        other.enabled = false;
        PlayerController.instance.onPassFloors.Remove(other);
    }
}
