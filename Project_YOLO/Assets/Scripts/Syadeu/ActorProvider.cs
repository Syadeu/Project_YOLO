using UnityEngine;

namespace Syadeu
{
    public sealed class ActorProvider<T> where T : MonoBehaviour, IActor
    {
        private readonly T m_Actor;
        private readonly SkillProvider<T> m_SkillProvider;
        private readonly InventoryProvider<T> m_InventoryProvider;

        public SkillProvider<T> SkillProvider => m_SkillProvider;

        public ActorProvider(T actor)
        {
            m_Actor = actor;
            m_SkillProvider = new SkillProvider<T>(actor);
            m_InventoryProvider = new InventoryProvider<T>(actor);
        }
    }
}