using Syadeu.Presentation;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Syadeu.Mono
{
    public sealed class GameStartComponent : MonoBehaviour
    {
        public float loadingWait = 2;

        public float m_FakeWaitDelay = 0;
        public int m_StartDelay = 2;

        public UnityEvent UnityEvent;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(loadingWait);

            PresentationSystem<SceneSystem>.System.LoadScene(0, m_FakeWaitDelay, m_StartDelay);
            UnityEvent?.Invoke();
        }
    }
}
