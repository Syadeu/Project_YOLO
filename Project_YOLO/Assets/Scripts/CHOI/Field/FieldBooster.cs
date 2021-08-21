using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBooster : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (PlayerController.instance.booster == null) return;
        if (PlayerController.instance.booster.haveBooster) return;

        PlayerController.instance.booster.BoosterAcquisition();
        Destroy(gameObject);
    }
}
