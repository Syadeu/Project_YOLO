using UnityEngine;

namespace Syadeu
{
    [RequireComponent(typeof(Collider))]
    public sealed class SavePointComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            YOLOPresentationProvider.Instance.GameSystem.m_CurrentPoint = transform;
        }
    }
}