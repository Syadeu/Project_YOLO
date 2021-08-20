using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
    {
        var tag = other.gameObject.tag;
        if (!other.gameObject.CompareTag("PassFloor")) return;
        if (other.isTrigger) return;
        if (PlayerController.instance.onPassFloors.Contains(other)) return;
        
        PlayerController.instance.onPassFloors.Add(other);
    }
    
    private void OnTriggerExit (Collider other)
    {
        PlayerController.instance.onPassFloors.Remove(other);
    }
}
