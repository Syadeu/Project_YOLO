using Newtonsoft.Json;
using Syadeu.Internal;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using System;

namespace Syadeu
{
    [AttributeAcceptOnly(null)]
    public sealed class DialogueReference : AttributeBase
    {
        public enum Culture
        {
            None    =   0,
            Korean  =   0b001,
            English =   0b010,

            All     =   ~0
        }

        [Serializable]
        public sealed class Text
        {
            public Culture Culture;
            [ReflectionDescription("말하는 주체")]
            public Reference<YOLOActorEntity> Principle;
            public string Message;
            [ReflectionDescription("말하고나서 기다릴 시간")]
            public float Delay;
        }

        [JsonProperty(PropertyName = "Texts")]
        public Text[] m_Texts = Array.Empty<Text>();
    }
}