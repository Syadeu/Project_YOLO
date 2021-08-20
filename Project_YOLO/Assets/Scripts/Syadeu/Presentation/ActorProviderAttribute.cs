using Newtonsoft.Json;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(YOLOActorEntity))]
    public sealed class ActorProviderAttribute : AttributeBase
    {
        [JsonProperty] public Reference<UIObjectEntity> m_ConversationUI;

        [JsonIgnore] internal ActorProviderBase m_ActorProvider;
    }
}