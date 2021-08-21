using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBooster : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (PlayerController.instance.Booster == null) return;
        if (PlayerController.instance.Booster.HaveBooster) return;

        PlayerController.instance.Booster.BoosterAcquisition();
        Destroy(gameObject);
    }
}
