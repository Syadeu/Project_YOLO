using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(Collider))]
    public sealed class CameraTriggerBlock : MonoBehaviour
    {
        public string CameraTargetName = string.Empty;
        public GameObject targetObject;

        private bool a;

        private void OnTriggerEnter(Collider other)
        {
            if (a || !other.gameObject.CompareTag("Player")) return;

            if (!CameraManager.HasInstance)
            {
                "카메라가 없음!!".ToLogError();
                return;
            }

            CameraManager.Instance.SetCameraTarget(CameraTargetName);
            targetObject.SetActive(true);
            a = true;
        }
    }
}
