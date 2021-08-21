using Syadeu.Presentation;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    public sealed class DestroyEntityAction : YOLOActionBase
    {
        public override void Process(EntityData<IEntityData> entity)
        {
            entity.Destroy();
        }
    }
}