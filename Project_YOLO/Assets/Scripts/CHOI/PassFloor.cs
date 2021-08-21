using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassFloor : MonoBehaviour
{
    [Space(5)] [Header("기본 정보")]
    [SerializeField] private Collider collider;

    private void OnTriggerStay(Collider other)
    {
        if (collider.enabled) return;
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (other.transform.position.y >= transform.position.y + (transform.localScale.y * 0.5f))
        {
            collider.enabled = true;
            PlayerController.instance.ONPassFloors.Add(collider);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (!PlayerController.instance.ONPassFloors.Contains(collider) && 
            other.transform.position.y >= transform.position.y + (transform.localScale.y * 0.5f))
        {
            collider.enabled = true;
            PlayerController.instance.ONPassFloors.Add(collider);
        }
        else
        {
            collider.enabled = false;
            PlayerController.instance.ONPassFloors.Remove(collider);
        }
    }
}
