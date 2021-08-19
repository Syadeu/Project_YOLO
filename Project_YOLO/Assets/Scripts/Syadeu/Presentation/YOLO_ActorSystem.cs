using Syadeu.Database;
using Syadeu.Internal;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Syadeu
{
    [SubSystem(typeof(EntitySystem))]
    public sealed class YOLO_ActorSystem : PresentationSystemEntity<YOLO_ActorSystem>
    {
        public override bool EnableBeforePresentation => true;
        public override bool EnableOnPresentation => false;
        public override bool EnableAfterPresentation => false;

        private readonly Queue<RegistryPayload> m_Registries = new Queue<RegistryPayload>();
        private readonly List<EntityData<YOLOActorEntity>> m_Actors = new List<EntityData<YOLOActorEntity>>();
        
        private EntitySystem m_EntitySystem;

        #region Presentation Methods

        protected override PresentationResult OnInitialize()
        {
            PoolContainer<ConversationHandler>.Initialize(ConversationHandlerFactory, 1);

            RequestSystem<EntitySystem>(Bind);

            return base.OnInitialize();
        }
#pragma warning disable CS0618 // Type or member is obsolete
        private ConversationHandler ConversationHandlerFactory() => new ConversationHandler();
#pragma warning restore CS0618 // Type or member is obsolete
        private void Bind(EntitySystem other)
        {
            m_EntitySystem = other;
        }
        public override void OnDispose()
        {
            for (int i = 0; i < m_Actors.Count; i++)
            {
                m_Actors[i].Destroy();
            }
            m_Actors.Clear();

            m_EntitySystem = null;
        }

        protected override PresentationResult BeforePresentation()
        {
            int registriesCount = m_Registries.Count;
            for (int i = 0; i < registriesCount; i++)
            {
                RegistryPayload temp = m_Registries.Dequeue();

                EntityData<IEntityData> entity = m_EntitySystem.CreateObject(temp.actor.ActorID.EntityHash);

                EntityData<YOLOActorEntity> converted = EntityData<YOLOActorEntity>.GetEntityData(entity.Idx);

                $"{entity.IsValid()} : {converted.IsValid()}".ToLog();

                temp.actorProvider.Initialize(converted);
                m_Actors.Add(converted);
            }

            return base.BeforePresentation();
        }

        #endregion

        public ActorProvider<T> RegisterActor<T>(T actor) where T : MonoBehaviour, IActor
        {
            if (actor.ActorID == null)
            {
                CoreSystem.Logger.LogError(Channel.Presentation,
                    $"ActorID 가 없는 actor({actor.name})");
                return null;
            }

            ActorProvider<T> provider = new ActorProvider<T>(this, actor);
            m_Registries.Enqueue(new RegistryPayload()
            {
                actor = actor,
                actorProvider = provider
            });
            return provider;
        }

        #region Inner Classes

        private sealed class RegistryPayload
        {
            public IActor actor;
            public ActorProviderBase actorProvider;

        }

        #endregion
    }
}