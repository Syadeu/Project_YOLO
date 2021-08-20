using System.Collections;
using System.Collections.Generic;
using Syadeu;
using UnityEngine;

public class FieldBlueprint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!other.isTrigger) return;
        
        PlayerController.instance.blueprintCount++;
        UIManager.Instance.SetBlueprintCount(PlayerController.instance.blueprintCount);
        Destroy(gameObject);
    }
}
