using Newtonsoft.Json;

namespace Syadeu
{
    public sealed class YOLOActorEntity : GhostEntityBase
    {
        [JsonProperty(Order = 0, PropertyName = "Type")] private ActorType m_ActorType;

        [JsonIgnore] internal ActorProviderBase m_ActorProvider;

        public ActorType ActorType => m_ActorType;
    }
}