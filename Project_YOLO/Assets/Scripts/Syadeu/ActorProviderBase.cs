﻿using Syadeu.Presentation.Entities;
using System;

namespace Syadeu
{
    public abstract class ActorProviderBase : ProviderBase, IDisposable
    {
        public abstract EntityData<YOLOActorEntity> Entity { get; }

        public abstract void Initialize(EntityData<YOLOActorEntity> entity);

        void IDisposable.Dispose() => OnDispose();
        protected virtual void OnDispose() { }
    }

    public abstract class ProviderBase { }
}