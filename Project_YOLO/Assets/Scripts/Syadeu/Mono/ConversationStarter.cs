using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(Collider))]
    public sealed class ConversationStarter : MonoBehaviour
    {
        public enum TriggerType
        {
            OnColliderEnter     =   0,
            OnColliderExit      =   1,
            OnStart             =   2
        }

        public TriggerType m_TriggerType = 0;
        public DialogueID m_DialogueID;
        public ActorRegistryComponent[] m_Entries = Array.Empty<ActorRegistryComponent>();
        public bool m_TriggerOnce = true;

        public bool Triggered { get; private set; } = false;

        private IEnumerator Start()
        {
            if (m_TriggerType != TriggerType.OnStart) yield break;

            while (!EntryCheck())
            {
                yield return null;
            }

            StartConversation();

            bool EntryCheck()
            {
                for (int i = 0; i < m_Entries.Length; i++)
                {
                    if (m_Entries[i].ActorProvider == null ||
                        !m_Entries[i].ActorProvider.IsInitialized) return false;
                }
                return true;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (m_TriggerType != 0) return;
            if (!other.gameObject.CompareTag("Player")) return;
            if (m_DialogueID == null)
            {
                "DialogueID 가 없는 스타터!".ToLogError(this);
                return;
            }

            StartConversation();
        }
        private void OnTriggerExit(Collider other)
        {
            if (m_TriggerType != TriggerType.OnColliderExit) return;
            if (!other.gameObject.CompareTag("Player")) return;
            if (m_DialogueID == null)
            {
                "DialogueID 가 없는 스타터!".ToLogError(this);
                return;
            }

            StartConversation();
        }

        private void StartConversation()
        {
            if (m_TriggerOnce && Triggered) return;

            var temp = m_Entries.Select((other) => other.ActorProvider.Entity).ToArray();
            YOLOPresentationProvider.Instance.GameSystem.StartConversation(m_DialogueID,
                temp[0], temp);

            Triggered = true;
        }
    }
}
