using UnityEngine;

namespace Syadeu
{
    public sealed class DialogueProvider<T> : ProviderBase where T : MonoBehaviour, IActor
    {
        private ActorProviderBase m_Actor;

        public DialogueProvider(ActorProviderBase provider)
        {
            m_Actor = provider;
        }
    }
}