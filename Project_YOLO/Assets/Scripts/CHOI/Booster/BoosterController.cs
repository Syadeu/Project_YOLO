using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    private Rigidbody rigidbody;
    
    [Space(5)] [Header("기본 정보")]
    public bool haveBooster;
    [SerializeField] private float boosterPower;
    [SerializeField] private float boosterCooltime;
    public bool boosterAvailable;
    public float maxVelocity = 10;
    [SerializeField] private GameObject boosterEffect;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void Booster()
    {
        if (!haveBooster) return;
        if (!boosterAvailable) return;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            boosterAvailable = false;

            rigidbody.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                maxVelocity = 10;
                rigidbody.AddForce(new Vector3(-1, 1.5f, 0) * (boosterPower * 0.7f), ForceMode.Impulse);
            }
            else
            {
                maxVelocity = 15;
                rigidbody.AddForce(Vector3.left * boosterPower, ForceMode.Impulse);
            }

            //부스터 이펙트
            BoosterEffect();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            boosterAvailable = false;

            rigidbody.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                maxVelocity = 10;
                rigidbody.AddForce(new Vector3(1, 1.5f, 0) * (boosterPower * 0.7f), ForceMode.Impulse);
            }
            else
            {
                maxVelocity = 15;
                rigidbody.AddForce(Vector3.right * boosterPower, ForceMode.Impulse);
            }

            //부스터 이펙트
            BoosterEffect();
        }
        else
        {
            boosterAvailable = false;

            maxVelocity = 15;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(new Vector3(0, 1, 0) * (boosterPower * 1.3f), ForceMode.Impulse);

            //부스터 이펙트
            BoosterEffect();
        }

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
        haveBooster = true;
        BoosterAvailable();
    }

    /// <summary>
    /// 부스터를 사용 가능하도록 상태 변경합니다
    /// </summary>
    private void BoosterAvailable()
    {
        //부스터 게이지 UI 적용
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
