using Newtonsoft.Json;
using Syadeu.Internal;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    internal sealed class YOLO_SystemGroup : PresentationGroupEntity
    {
        public override SceneReference DependenceScene => SceneList.Instance.Scenes[0];

        public override void Register()
        {
            RegisterSystem(
                TypeHelper.TypeOf<YOLO_GameSystem>.Type,
                TypeHelper.TypeOf<YOLO_ActorSystem>.Type
                );
        }
    }

    public sealed class DialogueEntity : EntityDataBase
    {
        [JsonProperty] public Reference<DialogueAttribute> Dialogue;

        public override bool IsValid() => true;
    }

    [AttributeAcceptOnly(typeof(DialogueEntity))]
    public sealed class DialogueAttribute : AttributeBase
    {

    }

    [AttributeAcceptOnly(null)]
    public abstract class ActionTriggerBase : AttributeBase { }
}