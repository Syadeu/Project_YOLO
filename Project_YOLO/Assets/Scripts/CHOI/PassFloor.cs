using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassFloor : MonoBehaviour
{
    [Space(5)] [Header("기본 정보")]
    [SerializeField] private Collider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (other.transform.position.y >= transform.position.y + (transform.localScale.y * 0.5f))
        {
            collider.enabled = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (other.transform.position.y > transform.position.y + (transform.localScale.y * 0.5f))
        {
            collider.enabled = true;
        }
        else
        {
            PlayerController.instance.onPassFloors.Remove(collider);
            collider.enabled = false;
        }
    }
}
