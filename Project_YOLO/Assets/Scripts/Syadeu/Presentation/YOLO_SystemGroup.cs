using Syadeu.Internal;
using Syadeu.Mono;
using Syadeu.Presentation;

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
}