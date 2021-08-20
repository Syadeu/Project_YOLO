using Newtonsoft.Json;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(YOLOActorEntity))]
    public sealed class ActorProviderAttribute : AttributeBase
    {
        [JsonIgnore] internal ActorProviderBase m_ActorProvider;
    }
}