using Syadeu.Internal;
using Syadeu.Presentation;

namespace Syadeu
{
    internal sealed class YOLO_SystemGroup : PresentationGroupEntity
    {
        public override bool StartOnInitialize => true;

        public override void Register()
        {
            RegisterSystem(
                TypeHelper.TypeOf<YOLO_ActorSystem>.Type
                );
        }
    }
}