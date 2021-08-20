using Newtonsoft.Json;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    public sealed class AnimationTriggerAction : YOLOActionBase
    {
        [JsonProperty] public string TriggerKey;

        public override void Process(EntityData<IEntityData> e)
        {
            ActorProviderAttribute provider = e.GetAttribute<ActorProviderAttribute>();
            if (provider == null)
            {
                "provider 가ㅣ 없음".ToLog();
                return;
            }


            //YOLOActorEntity entity = (YOLOActorEntity)e.Target;
            provider.m_ActorProvider.Animator.SetTrigger(TriggerKey);
        }
    }
}