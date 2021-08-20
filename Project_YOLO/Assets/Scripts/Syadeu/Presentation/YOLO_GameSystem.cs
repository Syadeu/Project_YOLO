﻿using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using Syadeu.Presentation.Events;
using System.Collections;
using UnityEngine;

namespace Syadeu
{
    [SubSystem(typeof(EventSystem))]
    public sealed class YOLO_GameSystem : PresentationSystemEntity<YOLO_GameSystem>
    {
        public override bool EnableBeforePresentation => false;
        public override bool EnableOnPresentation => false;
        public override bool EnableAfterPresentation => false;

        public EntityData<YOLOActorEntity> Player => m_YOLOActorSystem.Player;
        public ActorProviderAttribute PlayerProviderAttribute => Player.GetAttribute<ActorProviderAttribute>();
        public DialogueAttribute PlayerDialogueAttribute => Player.GetAttribute<DialogueAttribute>();

        public bool IsInConversation { get; private set; } = false;

        private EventSystem m_EventSystem;
        private YOLO_ActorSystem m_YOLOActorSystem;

        protected override PresentationResult OnInitialize()
        {
            YOLOPresentationProvider.Instance.GameSystem = this;

            RequestSystem<EventSystem>(Bind);
            RequestSystem<YOLO_ActorSystem>(Bind);

            return base.OnInitialize();
        }
        public override void OnDispose()
        {
            m_EventSystem.RemoveEvent<OnItemLootEvent>(OnItemLootEventHandler);
            m_EventSystem.RemoveEvent<OnItemDropEvent>(OnItemDropEventHandler);

            m_EventSystem = null;
            m_YOLOActorSystem = null;
        }

        #region Bind

        private void Bind(EventSystem other)
        {
            m_EventSystem = other;
            m_EventSystem.AddEvent<OnItemLootEvent>(OnItemLootEventHandler);
            m_EventSystem.AddEvent<OnItemDropEvent>(OnItemDropEventHandler);
        }
        private void OnItemLootEventHandler(OnItemLootEvent ev)
        {
            if ((ev.Character.Entity.Target.ActorType & ActorType.Player) == ActorType.Player)
            {
                if (ev.Item.ItemType == ItemType.Seed)
                {
                    "씨앗을 얻었다!".ToLog();
                }
            }
        }
        private void OnItemDropEventHandler(OnItemDropEvent ev)
        {
            if ((ev.Character.Entity.Target.ActorType & ActorType.Player) == ActorType.Player)
            {
                if (ev.Item.ItemType == ItemType.Seed)
                {
                    "씨앗을 떨궜다!".ToLog();
                }
            }
        }

        private void Bind(YOLO_ActorSystem other)
        {
            m_YOLOActorSystem = other;
        }

        #endregion

        public ConversationHandler StartPlayerConversation(DialogueID dialogueID, params EntityData<YOLOActorEntity>[] joinedEntities)
        {
            if (IsInConversation)
            {
                "뭔가 대화중임".ToLogError();
                return null;
            }

            PlayerDialogueAttribute.TryConversation(dialogueID, out ConversationHandler handler, joinedEntities);

            StartCoroutine(ConversationUpdate(handler));
            return handler;
        }
        public ConversationHandler StartConversation(DialogueID dialogueID, EntityData<YOLOActorEntity> speaker, params EntityData<YOLOActorEntity>[] joinedEntities)
        {
            if (IsInConversation)
            {
                "뭔가 대화중임".ToLogError();
                return null;
            }

            var dialogueAtt = speaker.GetAttribute<DialogueAttribute>();
            if (dialogueAtt == null)
            {
                $"dialouge 어트리뷰트가 없음, {speaker.Name}".ToLog();
                return null;
            }

            dialogueAtt.TryConversation(dialogueID, out ConversationHandler handler, joinedEntities);

            StartCoroutine(ConversationUpdate(handler));
            return handler;
        }
        private IEnumerator ConversationUpdate(ConversationHandler handler)
        {
            IsInConversation = true;
            float timer = 0;

            handler.StartConversation(Conversation, out float delay);

            while (timer < delay)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    "skip".ToLog();
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (delay > 0 && timer > 0)
            {
                if (!handler.MoveNext(out delay))
                {
                    "대화 끝".ToLog();
                    IsInConversation = false;
                    yield break;
                }
            }

            timer = 0;

            while (true)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (!handler.MoveNext(out delay))
                    {
                        "대화 끝".ToLog();
                        IsInConversation = false;
                        yield break;
                    }
                    timer = 0;
                }

                while (timer < delay)
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        "skip".ToLog();
                        break;
                    }

                    timer += Time.deltaTime;
                    yield return null;
                }

                if (delay > 0 && timer > 0)
                {
                    if (!handler.MoveNext(out delay))
                    {
                        "대화 끝".ToLog();
                        IsInConversation = false;
                        yield break;
                    }

                    timer = 0;
                }

                yield return null;
            }

            void Conversation(EntityData<YOLOActorEntity> entity, string text)
            {
                $"{entity.Name} 이 {text} 를 말함".ToLog();
            }
        }
    }
}