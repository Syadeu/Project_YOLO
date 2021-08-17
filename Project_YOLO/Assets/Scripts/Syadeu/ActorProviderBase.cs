using System;

namespace Syadeu
{
    public abstract class ActorProviderBase : IDisposable
    {
        public abstract ActorType ActorType { get; }

        void IDisposable.Dispose() => OnDispose();
        protected virtual void OnDispose() { }
    }
}