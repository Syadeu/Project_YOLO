using Newtonsoft.Json;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;
using System.Collections;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(EntityBase))]
    public sealed class SynchronizeAlwaysAttribute : AttributeBase
    {
        [JsonIgnore] public CoreRoutine routine;
    }
    internal sealed class SynchronizeAlwaysProcessor : AttributeProcessor<SynchronizeAlwaysAttribute>,
        IAttributeOnProxy
    {
        public void OnProxyCreated(AttributeBase attribute, Entity<IEntity> entity, RecycleableMonobehaviour monoObj)
        {
            SynchronizeAlwaysAttribute att = (SynchronizeAlwaysAttribute)attribute;

            att.routine = CoreSystem.StartUnityUpdate(this, Update(entity));
        }
        public void OnProxyRemoved(AttributeBase attribute, Entity<IEntity> entity, RecycleableMonobehaviour monoObj)
        {
            SynchronizeAlwaysAttribute att = (SynchronizeAlwaysAttribute)attribute;

            if (att.routine.IsValid() && att.routine.IsRunning)
            {
                CoreSystem.RemoveUnityUpdate(att.routine);
            }
        }

        private IEnumerator Update(Entity<IEntity> entity)
        {
            while (entity.IsValid() && entity.hasProxy)
            {
                IProxyTransform tr = (IProxyTransform)entity.transform;
                tr.Synchronize(ProxyTransform.SynchronizeOption.TRS);

                yield return null;
            }
        }
    }
}