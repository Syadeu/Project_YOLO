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
        private readonly List<ActorProviderBase> m_Actors = new List<ActorProviderBase>();
        
        private EntitySystem m_EntitySystem;

        #region Presentation Methods

        protected override PresentationResult OnInitialize()
        {
            RequestSystem<EntitySystem>(Bind);

            return base.OnInitialize();
        }
        private void Bind(EntitySystem other)
        {
            m_EntitySystem = other;
        }
        public override void OnDispose()
        {
            for (int i = 0; i < m_Actors.Count; i++)
            {
                ((IDisposable)m_Actors[i]).Dispose();
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

                temp.actorProvider.Initialize(converted);
                m_Actors.Add(temp.actorProvider);
            }

            return base.BeforePresentation();
        }

        #endregion

        public void RegisterActor<T>(T actor) where T : MonoBehaviour, IActor
        {
            if (actor.ActorID == null)
            {
                CoreSystem.Logger.LogError(Channel.Presentation,
                    $"ActorID 가 없는 actor({actor.name})");
                return;
            }

            m_Registries.Enqueue(new RegistryPayload()
            {
                actor = actor,
                actorProvider = new ActorProvider<T>(this, actor)
            });
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