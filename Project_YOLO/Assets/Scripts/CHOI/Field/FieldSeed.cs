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
        if (PlayerController.instance.Gun == null) return;
        if (PlayerController.instance.Gun.HaveSeed) return;

        PlayerController.instance.Gun.SetSeedStatus(true);
        Destroy(gameObject);
    }
}