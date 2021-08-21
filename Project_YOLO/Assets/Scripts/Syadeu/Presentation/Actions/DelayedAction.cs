using Newtonsoft.Json;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using System.Collections;
using UnityEngine;

namespace Syadeu
{
    public sealed class DelayedAction : YOLOActionBase
    {
        [JsonProperty] private Reference<YOLOActionBase> Action;
        [JsonProperty] private float Delay;

        public override void Process(EntityData<IEntityData> entity)
        {
            if (!Action.IsValid())
            {
                $"{entity.Name} 에서 유효하지않은 액션을 실행하려함".ToLogError();
                return;
            }

            CoreSystem.StartUnityUpdate(this, DelayCall(Delay, Action, entity));
        }
        private IEnumerator DelayCall(float time, Reference<YOLOActionBase> action, EntityData<IEntityData> entity)
        {
            float timer = 0;
            while (time > 0 && timer < time)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            action.GetObject().Process(entity);
        }
    }
}