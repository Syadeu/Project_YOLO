using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    [AttributeAcceptOnly(null)]
    public abstract class YOLOActionBase : AttributeBase
    {
        public abstract void Process(EntityData<IEntityData> entity);
    }
}