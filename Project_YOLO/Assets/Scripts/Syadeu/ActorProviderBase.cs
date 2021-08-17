using System;

namespace Syadeu
{
    public abstract class ActorProviderBase : IDisposable
    {
        public abstract ActorType ActorType { get; }

        public abstract void Dispose();
    }
}