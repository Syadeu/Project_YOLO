using UnityEngine;

namespace Syadeu
{
    public sealed class ActorProvider<T> : ActorProviderBase where T : MonoBehaviour, IActor
    {
        private T m_Actor;
        private StatProvider<T> m_StatProvider;
        private SkillProvider<T> m_SkillProvider;
        private InventoryProvider<T> m_InventoryProvider;

        public override ActorType ActorType => m_Actor.ActorID.ActorType;

        public StatProvider<T> StatProvider => m_StatProvider;
        public SkillProvider<T> SkillProvider => m_SkillProvider;
        public InventoryProvider<T> InventoryProvider => m_InventoryProvider;

        public ActorProvider(T actor)
        {
            m_Actor = actor;
            m_StatProvider = new StatProvider<T>(actor);
            m_SkillProvider = new SkillProvider<T>(actor);
            m_InventoryProvider = new InventoryProvider<T>(actor);
        }

        public override void Dispose()
        {
            m_SkillProvider.Dispose();
            m_InventoryProvider.Dispose();

            m_Actor = null;
            m_StatProvider = null;
            m_SkillProvider = null;
            m_InventoryProvider = null;
        }
    }
}