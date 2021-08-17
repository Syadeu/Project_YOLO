using Syadeu.Internal;
using Syadeu.Presentation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Syadeu
{
    public sealed class YOLO_ActorSystem : PresentationSystemEntity<YOLO_ActorSystem>
    {
        public override bool EnableBeforePresentation => false;
        public override bool EnableOnPresentation => false;
        public override bool EnableAfterPresentation => false;

        private readonly List<ActorProviderBase> m_Actors = new List<ActorProviderBase>();

        #region Presentation Methods

        public override void OnDispose()
        {
            for (int i = 0; i < m_Actors.Count; i++)
            {
                m_Actors[i].Dispose();
            }
            m_Actors.Clear();
        }

        #endregion

        public ActorProvider<T> RegisterActor<T>(T actor) where T : MonoBehaviour, IActor
        {
            if (actor.ActorID == null)
            {
                throw new Exception($"ActorID 가 없는 actor({actor.name})");
            }

            ActorProvider<T> provider = new ActorProvider<T>(actor);
            m_Actors.Add(provider);
            return provider;
        }
    }
}