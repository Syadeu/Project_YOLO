using Newtonsoft.Json;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(EntityBase))]
    public sealed class TransformFollowerAttribute : AttributeBase
    {
        [JsonIgnore] public float FollowSpeed = 4;

        [JsonIgnore] public bool Set = false;
        [JsonIgnore] public Transform Target = null;
        [JsonIgnore] public float3 Offset = 0;

        public void Setup(Transform tr, float3 offset)
        {
            Target = tr;
            Offset = offset;
            Set = true;
        }
    }
    internal sealed class TransformFollowerProcessor : AttributeProcessor<TransformFollowerAttribute>,
        IAttributeOnProxyCreated
    {
        public void OnProxyCreated(AttributeBase attribute, Entity<IEntity> entity, RecycleableMonobehaviour monoObj)
        {
            TransformFollowerAttribute att = (TransformFollowerAttribute)attribute;
            if (att.Target == null) return;

            CoreSystem.StartUnityUpdate(this, Update(entity, att));
        }

        private static IEnumerator Update(Entity<IEntity> entity, TransformFollowerAttribute att)
        {
            while (entity.IsValid())
            {
                if (!att.Set || !entity.hasProxy)
                {
                    yield return null;
                    continue;
                }

                if (entity.IsValid())
                {
                    float3 pos = math.lerp(entity.transform.position, (float3)att.Target.position + att.Offset, Time.deltaTime * att.FollowSpeed);

                    entity.transform.position = pos;
                    //entity.transform.position = (float3)att.Target.position + att.Offset;
                }
                else break;

                yield return null;
            }

            "break".ToLog();
        }
    }
}