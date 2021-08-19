﻿using Syadeu.Database;
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

        public bool TryConversation(DialogueID id, ActorProviderBase target, out ConversationHandler handler)
        {
            $"{Entity.IsValid()}".ToLog();

            handler = null;
            if (id.AttributeHash.Equals(0))
            {
                "ID Hash 가 지정되지 않은 DialogueID".ToLog(id);
                return false;
            }

            DialogueAttribute dialogueAtt = target.Entity.GetAttribute<DialogueAttribute>();
            if (dialogueAtt == null)
            {
                $"{target.Entity.Name} 은 대화가 가능한 상대가 아님".ToLog();
                return false;
            }

            DialogueReference dialogue = dialogueAtt.GetDialogues(id);
            if (dialogue.m_Texts.Length == 0)
            {
                "아무 대화도 없음".ToLog();
                return false;
            }
            else if (!dialogue.m_Texts[0].Principle.m_Hash.Equals(Entity.Target.Hash))
            {
                "대화 주체가 아님".ToLog();
                return false;
            }

            if (!dialogue.HasEntity(target.Entity.Hash))
            {
                $"{target.Entity.Name} 은 대화({dialogue.Name})에서 할말이 없는데 대화하려함".ToLog();
                return false;
            }

            handler = PoolContainer<ConversationHandler>.Dequeue();
            handler.Initialize(dialogue, Entity, target.Entity);
            return true;
        }
    }
}