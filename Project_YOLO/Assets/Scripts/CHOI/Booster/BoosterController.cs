using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoosterType 
{
    Straight,
    Diagonal,
    Upward
}

public class BoosterController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    [Space(5)] [Header("직선 부스터 파워")]
    [SerializeField] private float straightPower;
    
    [Space(5)] [Header("대각 부스터 파워")]
    [SerializeField] private float diagonalPower;
    
    [Space(5)] [Header("위로 부스터 파워")]
    [SerializeField] private float upPower;
    
    
    [Space(5)] [Header("기본 정보")]
    [SerializeField] private float boosterCooltime;
    [SerializeField] private GameObject boosterEffect;
    
    //부스터 상태
    [NonSerialized] public bool HaveBooster;
    [NonSerialized] public bool boosterAvailable;

    private IEnumerator Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        yield return new WaitUntil(() => UIManager.HasInstance);
        UIManager.Instance.EnableBoostGage(false);
    }

    public void Booster(BoosterType type, Vector3 force)
    {
        boosterAvailable = false;
        
        switch (type)
        {
            case BoosterType.Straight:
                _rigidbody.AddForce(force * straightPower, ForceMode.Impulse);
                break;
            case BoosterType.Diagonal:
                _rigidbody.AddForce(force * diagonalPower, ForceMode.Impulse);
                break;
            case BoosterType.Upward:
                _rigidbody.AddForce(force * upPower, ForceMode.Impulse);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        //부스터 이펙트
        BoosterEffect();
        
        //쿨타임 시작
        StartCoroutine(BoosterCooltime());
    }

    private void BoosterEffect()
    {
        //이펙트
        boosterEffect.gameObject.SetActive(false);
        boosterEffect.gameObject.SetActive(true);
    }

    /// <summary>
    /// 부스터 획득 시 호출
    /// </summary>
    public void BoosterAcquisition()
    {
        HaveBooster = true;
        BoosterAvailable();
    }

    /// <summary>
    /// 부스터를 사용 가능하도록 상태 변경합니다
    /// </summary>
    private void BoosterAvailable()
    {
        //부스터 게이지 UI 적용
        UIManager.Instance.EnableBoostGage(true);
        UIManager.Instance.SetBoostGage(1);

        boosterAvailable = true;
    }
    
    //부스터 쿨타임
    IEnumerator BoosterCooltime()
    {
        var wait = new WaitForSeconds(0.01f);
        
        float time = 0;
        while (time < boosterCooltime)
        {
            //부스터 게이지 UI 적용
            UIManager.Instance.SetBoostGage(time / boosterCooltime);

            yield return wait;

            time += 0.01f;
        }
        
        BoosterAvailable();
    }
}
