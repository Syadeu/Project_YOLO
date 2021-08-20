using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor")) return;
        
        PlayerController.instance.CollisionException(false);
    }
}
