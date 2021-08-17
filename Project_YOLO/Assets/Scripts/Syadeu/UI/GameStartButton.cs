using Syadeu.Presentation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartButton : MonoBehaviour
{
    public float m_FakeWaitDelay = 0;
    public int m_StartDelay = 2;

    public void OnButtonClicked()
    {
        PresentationSystem<SceneSystem>.System.LoadScene(0, m_FakeWaitDelay, m_StartDelay);
    }
}
