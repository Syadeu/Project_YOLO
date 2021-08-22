using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DeadTriggerComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var pos = YOLOPresentationProvider.Instance.GameSystem.GetLastSavePosition();
            PlayerController.instance.transform.position = pos;
        }
    }
}
