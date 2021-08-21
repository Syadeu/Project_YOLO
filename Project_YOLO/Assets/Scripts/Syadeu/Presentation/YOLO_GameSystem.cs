using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using Syadeu.Presentation.Events;
using Syadeu.Presentation.Render;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Syadeu
{
    [SubSystem(typeof(EventSystem))]
    public sealed class YOLO_GameSystem : PresentationSystemEntity<YOLO_GameSystem>
    {
        public override bool EnableBeforePresentation => false;
        public override bool EnableOnPresentation => false;
        public override bool EnableAfterPresentation => false;

        public override bool IsStartable
        {
            get
            {
                if (!CameraManager.HasInstance) return false;
                return true;
            }
        }

        public EntityData<YOLOActorEntity> Player => m_YOLOActorSystem.Player;
        public ActorProviderAttribute PlayerProviderAttribute => Player.GetAttribute<ActorProviderAttribute>();
        public DialogueAttribute PlayerDialogueAttribute => Player.GetAttribute<DialogueAttribute>();

        public bool IsInConversation { get; private set; } = false;

        private EventSystem m_EventSystem;
        private RenderSystem m_RenderSystem;
        private WorldCanvasSystem m_WorldCanvasSystem;
        private EntitySystem m_EntitySystem;
        private YOLO_ActorSystem m_YOLOActorSystem;

        #region Presentation Methods
        protected override PresentationResult OnInitialize()
        {
            YOLOPresentationProvider.Instance.GameSystem = this;

            RequestSystem<EventSystem>(Bind);
            RequestSystem<RenderSystem>(Bind);
            RequestSystem<WorldCanvasSystem>(Bind);
            RequestSystem<EntitySystem>(Bind);
            RequestSystem<YOLO_ActorSystem>(Bind);

            return base.OnInitialize();
        }
        public override void OnDispose()
        {
            m_EventSystem.RemoveEvent<OnItemLootEvent>(OnItemLootEventHandler);
            m_EventSystem.RemoveEvent<OnItemDropEvent>(OnItemDropEventHandler);

            m_EventSystem = null;
            m_RenderSystem = null;
            m_YOLOActorSystem = null;
            m_EntitySystem = null;
        }

        #region Bind

        private void Bind(RenderSystem other)
        {
            m_RenderSystem = other;
        }
        private void Bind(WorldCanvasSystem other)
        {
            m_WorldCanvasSystem = other;
        }

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
        private void Bind(EntitySystem other)
        {
            m_EntitySystem = other;
        }

        #endregion

        protected override PresentationResult OnStartPresentation()
        {
            m_RenderSystem.Camera = CameraManager.Instance.Camera;
            m_WorldCanvasSystem.Canvas.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 10;

            FMOD.FMODSystem.GetParameterDescriptionByName("GameState", out var description);
            FMOD.FMODSystem.SetGlobalParam(description, 2);

            return base.OnStartPresentation();
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

        Entity<IEntity> m_ConversationUI = Entity<IEntity>.Empty;

        private IEnumerator ConversationUpdate(ConversationHandler handler)
        {
            IsInConversation = true;
            float timer = 0;
            
            handler.StartConversation(Conversation, out float delay);

            yield return null;

            while (true)
            {
                if (delay > 0)
                {
                    while (timer < delay)
                    {
                        if (Input.GetKeyUp(KeyCode.Space))
                        {
                            "skip 2".ToLog();
                            timer = delay;
                            break;
                        }

                        timer += Time.deltaTime;
                        yield return null;
                    }
                    timer = 0;
                    yield return null;
                    continue;
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    bool skipped = false;
                    if (!m_ConversationUI.Equals(Entity<IEntity>.Empty))
                    {
                        var prevUI = m_ConversationUI.GetAttribute<ConversationUIAttribute>();
                        if (prevUI.UIComponent.IsTexting)
                        {
                            prevUI.UIComponent.Skip();
                            skipped = true;
                        }
                    }
                    if (!skipped && !handler.MoveNext(out delay))
                    {
                        "대화 끝 2".ToLog();
                        DestroyUI();
                        IsInConversation = false;
                        yield break;
                    }
                }

                yield return null;
            }

            void Conversation(EntityData<YOLOActorEntity> entity, string text)
            {
                DestroyUI();

                $"{entity.Name} 이 {text} 를 말함".ToLog();

                ActorProviderAttribute provider = entity.GetAttribute<ActorProviderAttribute>();

                float3 pos = provider.m_ActorProvider.Transform.position;
                pos += provider.m_ConvUIOffset;

                m_ConversationUI = m_EntitySystem.CreateEntity(provider.m_ConversationUI, pos);
                m_ConversationUI.GetAttribute<ConversationUIAttribute>().TargetText = text;

                m_ConversationUI.GetAttribute<TransformFollowerAttribute>().Setup(provider.m_ActorProvider.Transform, provider.m_ConvUIOffset);
            }
            void DestroyUI()
            {
                if (!m_ConversationUI.Equals(Entity<IEntity>.Empty)) m_EntitySystem.DestroyEntity(m_ConversationUI);
            }
        }
    }
}