using Syadeu.Presentation.Entities;
using System;
using UnityEngine;

namespace Syadeu
{
    public abstract class ActorProviderBase : ProviderBase, IDisposable
    {
        public abstract bool IsInitialized { get; }
        public abstract EntityData<YOLOActorEntity> Entity { get; }
        public abstract Transform Transform { get; }
        public abstract Animator Animator { get; }

        public abstract void Initialize(EntityData<YOLOActorEntity> entity);

        void IDisposable.Dispose() => OnDispose();
        protected virtual void OnDispose() { }
    }

    public abstract class ProviderBase { }
}