using Syadeu.Presentation;
using UnityEngine;
using UnityEngine.Events;

namespace Syadeu.Mono
{
    public sealed class GameStartComponent : MonoBehaviour
    {
        public float m_FakeWaitDelay = 0;
        public int m_StartDelay = 2;

        public UnityEvent UnityEvent;

        private void Start()
        {
            PresentationSystem<SceneSystem>.System.LoadScene(0, m_FakeWaitDelay, m_StartDelay);
            UnityEvent?.Invoke();
        }
    }
}
