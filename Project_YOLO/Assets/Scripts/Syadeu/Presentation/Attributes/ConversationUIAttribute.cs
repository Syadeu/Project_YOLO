using Newtonsoft.Json;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(UIObjectEntity))]
    public sealed class ConversationUIAttribute : AttributeBase
    {
        [JsonProperty] public float TextSpeed = .1f;

        [JsonIgnore] public string TargetText = string.Empty;
    }
    internal sealed class ConversationUIProcessor : AttributeProcessor<ConversationUIAttribute>,
        IAttributeOnProxyCreated
    {
        public void OnProxyCreated(AttributeBase attribute, Entity<IEntity> entity, RecycleableMonobehaviour monoObj)
        {
            ConversationUIAttribute att = (ConversationUIAttribute)attribute;
            monoObj.GetComponent<TextUIComponent>().StartText(att.TargetText, att.TextSpeed);
        }
    }
}