using Syadeu;
using Syadeu.Mono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoManager<UIManager>
{
    [SerializeField] private Slider m_BoostGageUI;
    [SerializeField] private SimpleSwitch m_SeedGunUI;
    [SerializeField] private Text m_BlueprintTextUI;

    /// <summary>
    /// UI 의 부스트 게이지 퍼센트를 설정합니다.
    /// </summary>
    /// <param name="persent">0 ~ 100%</param>
    public void SetBoostGage(float persent)
    {
        persent *= 0.01f;

        m_BoostGageUI.value = persent;
    }

    /// <summary>
    /// 씨앗총의 현재 상태를 설정합니다.
    /// </summary>
    /// <param name="hasSeed">씨앗 총알이 있나요?</param>
    public void SetSeedGun(bool hasSeed)
    {
        if (hasSeed) m_SeedGunUI.On(1);
        else m_SeedGunUI.On(0);
    }

    public void SetBlueprintCount(int count)
    {
        const string c_Base = "{0} / 4";
        m_BlueprintTextUI.text = string.Format(c_Base, count);
    }
}
