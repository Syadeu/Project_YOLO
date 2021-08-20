using Syadeu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSeed : MonoBehaviour, IItem
{
    public ItemType ItemType => ItemType.Seed;

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (PlayerController.instance.gun.haveSeed) return;

        PlayerController.instance.gun.SetSeedStatus(true);
        Destroy(gameObject);
    }
}