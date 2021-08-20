using Syadeu.Database;
using Syadeu.Presentation.Entities;
using System.Collections;
using UnityEngine;

namespace Syadeu
{
    public sealed class ActorProvider<T> : ActorProviderBase where T : MonoBehaviour, IActor
    {
        private YOLO_ActorSystem m_System; 
        private T m_Actor;
        private StatProvider<T> m_StatProvider;
        private SkillProvider<T> m_SkillProvider;
        private InventoryProvider<T> m_InventoryProvider;

        private bool m_IsInitialized = false;
        private Hash m_Entity;

        public override bool IsInitialized => m_IsInitialized;
        public override EntityData<YOLOActorEntity> Entity => m_Entity;
        public override Transform Transform => m_Actor.transform;
        public override Animator Animator => m_Actor.Animator;

        public StatProvider<T> StatProvider => m_StatProvider;
        public SkillProvider<T> SkillProvider => m_SkillProvider;
        public InventoryProvider<T> InventoryProvider => m_InventoryProvider;

        public ActorProvider(YOLO_ActorSystem system, T actor)
        {
            m_System = system;
            m_Actor = actor;
            m_StatProvider = new StatProvider<T>(actor);
            m_SkillProvider = new SkillProvider<T>(actor);
            m_InventoryProvider = new InventoryProvider<T>(actor, this);
        }
        public override void Initialize(EntityData<YOLOActorEntity> entity)
        {
            $"init {entity.Name}".ToLog();

            m_Entity = entity.Idx;
            m_IsInitialized = true;
        }

        protected override void OnDispose()
        {
            "disposed".ToLog();
            m_SkillProvider.Dispose();
            m_InventoryProvider.Dispose();

            m_System = null;
            m_Actor = null;
            m_StatProvider = null;
            m_SkillProvider = null;
            m_InventoryProvider = null;
        }

        /// <summary>
        /// 이 캐릭터에 데미지를 입힙니다.
        /// </summary>
        /// <param name="damage">데미지 값</param>
        /// <returns>해당 데미지를 입고 죽었나요?</returns>
        public bool TakeDamage(float damage)
        {
            m_StatProvider.HP -= damage;
            if (m_StatProvider.HP <= 0) return true;
            return false;
        }

        public bool TryConversation(DialogueID id, out ConversationHandler handler, params EntityData<YOLOActorEntity>[] joinedEntities)
        {
            return Entity.GetAttribute<DialogueAttribute>().TryConversation(id, out handler, joinedEntities);
        }
    }
}