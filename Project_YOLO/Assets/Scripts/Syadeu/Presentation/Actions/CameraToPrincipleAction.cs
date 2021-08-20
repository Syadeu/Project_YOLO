using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    public sealed class CameraToPrincipleAction : YOLOActionBase
    {
        public override void Process(EntityData<IEntityData> e)
        {
            ActorProviderAttribute provider = e.GetAttribute<ActorProviderAttribute>();
            CameraManager.Instance.SetCameraTarget(new CameraManager.CameraTarget
            {
                m_Target = provider.m_ActorProvider.Transform
            });
        }
    }
}