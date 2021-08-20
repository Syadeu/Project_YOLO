
using System;
using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(Collider))]
    public sealed class CameraTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private Transition m_TransitionOne;
        [SerializeField] private Transition m_TransitionTwo;

        [SerializeField] private bool m_IsOne = false;
        [SerializeField] private bool m_Stay = false;

        [Serializable]
        public sealed class Transition
        {
            public bool UseName = true;
            public string CameraTargetName = string.Empty;

            [Space]
            public CameraManager.CameraTarget CameraTarget;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_Stay ||
                !other.gameObject.CompareTag("Player")) return;

            if (!CameraManager.HasInstance)
            {
                "카메라가 없음!!".ToLogError();
                return;
            }

            Transition transition = m_IsOne ? m_TransitionOne : m_TransitionTwo;

            if (transition.UseName)
            {
                CameraManager.Instance.SetCameraTarget(transition.CameraTargetName);
            }
            else
            {
                CameraManager.Instance.SetCameraTarget(transition.CameraTarget);
            }

            m_Stay = true;
        }
        private void OnTriggerExit(Collider other)
        {
            m_Stay = false;
            m_IsOne = !m_IsOne;
        }
    }
}
