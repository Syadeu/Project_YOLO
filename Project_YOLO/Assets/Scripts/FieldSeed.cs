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
        if (PlayerController.instance.haveSeed) return;
        
        PlayerController.instance.InsertItem(this);
        //Destroy(gameObject);
    }
}

// 이걸로 씨앗 얻고 하시는것 같은데, 파괴하지말고 인터페이스 달고 사용하시면 될거같아요
// 이런식으로요