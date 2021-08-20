using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor")) return;

        PlayerController.instance.CollisionException(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor")) return;
        
        PlayerController.instance.DownJumpEnd();
    }
}
