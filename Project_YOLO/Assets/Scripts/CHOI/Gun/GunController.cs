using Syadeu.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField] Transform muzzle;
    [SerializeField] private ProjectileSeed seedProjectile;
    
    //씨앗 상태
    [NonSerialized] public bool HaveSeed;

    public ObValue<bool> HasGun = new ObValue<bool>(ObValueDetection.Changed);

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UIManager.HasInstance);

        UIManager.Instance.EnableSeedGun(false);
        HasGun.OnValueChange += HasGun_OnValueChange;
    }

    private void HasGun_OnValueChange(bool current, bool target)
    {
        UIManager.Instance.EnableSeedGun(target);
    }

    private void Update()
    {
        if (PlayerController.instance.inputPause) return;
        
        Shoot();
    }
    
    /// <summary>
    /// 씨앗 발사 메소드
    /// </summary>
    private void Shoot()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.V)) return;
        if (!HaveSeed) return;

        SetSeedStatus(false);
        Instantiate(seedProjectile, muzzle.position, transform.rotation);
    }
    
    /// <summary>
    /// 씨앗 상태 변경
    /// </summary>
    public void SetSeedStatus(bool seedStatus)
    {
        HaveSeed = seedStatus;
        
        //씨앗 UI 변경
        UIManager.Instance.SetSeedGun(HaveSeed);
    }
}