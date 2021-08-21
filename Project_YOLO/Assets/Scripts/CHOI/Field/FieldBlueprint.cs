using System.Collections;
using System.Collections.Generic;
using Syadeu;
using UnityEngine;

public class FieldBlueprint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        PlayerController.instance.BlueprintCount++;
        UIManager.Instance.SetBlueprintCount(PlayerController.instance.BlueprintCount);
        Destroy(gameObject);
    }
}
