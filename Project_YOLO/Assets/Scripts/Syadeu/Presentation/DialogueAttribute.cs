using Newtonsoft.Json;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(YOLOActorEntity))]
    public sealed class DialogueAttribute : AttributeBase
    {
        [JsonProperty(Order = 0, PropertyName = "TargetEntity")]
        public Reference<YOLOActorEntity> m_TargetEntity;

        [JsonProperty(Order = 1, PropertyName = "Dialogue")]
        public Reference<DialogueReference> m_Dialogue;
    }
}