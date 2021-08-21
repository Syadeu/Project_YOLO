using Newtonsoft.Json;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;
using Unity.Mathematics;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(YOLOActorEntity))]
    public sealed class ActorProviderAttribute : AttributeBase
    {
        [JsonProperty] public Reference<UIObjectEntity> m_ConversationUI;
        [JsonProperty] public float3 m_ConvUIOffset;

        [JsonIgnore] internal ActorProviderBase m_ActorProvider;
    }
}