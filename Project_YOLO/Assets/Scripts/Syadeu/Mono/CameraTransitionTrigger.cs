
using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(Collider))]
    public sealed class CameraTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private bool m_UseName = true;
        [SerializeField] private string m_CameraTargetName = string.Empty;

        [Space]
        [SerializeField] private CameraManager.CameraTarget m_CameraTarget;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            if (!CameraManager.HasInstance)
            {
                "카메라가 없음!!".ToLogError();
                return;
            }

            if (m_UseName)
            {
                CameraManager.Instance.SetCameraTarget(m_CameraTargetName);
            }
            else
            {
                CameraManager.Instance.SetCameraTarget(m_CameraTarget);
            }
            //CameraManager.Instance.SetRoom()
        }
    }
}
