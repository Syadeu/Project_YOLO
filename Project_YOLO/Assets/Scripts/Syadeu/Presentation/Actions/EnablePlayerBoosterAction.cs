using Syadeu.Presentation;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    public sealed class EnablePlayerBoosterAction : YOLOActionBase
    {
        public override void Process(EntityData<IEntityData> entity)
        {
            PlayerController.instance.Booster.HaveBooster = true;
            PlayerController.instance.Booster.boosterAvailable = true;
        }
    }
}