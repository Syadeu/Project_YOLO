using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField] Transform muzzle;
    [SerializeField] private ProjectileSeed seedProjectile;
    
    [Space(5)] [Header("씨앗 상태")]
    public bool haveSeed;
    
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
        if (!Input.GetKeyDown(KeyCode.V)) return;
        if (!haveSeed) return;

        SetSeedStatus(false);
        Instantiate(seedProjectile, muzzle.position, PlayerController.instance.transform.rotation);
    }
    
    /// <summary>
    /// 씨앗 상태 변경
    /// </summary>
    public void SetSeedStatus(bool seedStatus)
    {
        haveSeed = seedStatus;
        
        //씨앗 UI 변경
        UIManager.Instance.SetSeedGun(haveSeed);
    }
}