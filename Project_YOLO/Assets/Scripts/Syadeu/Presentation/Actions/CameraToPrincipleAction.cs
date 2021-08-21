using Syadeu.Internal;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    [ReflectionDescription("DialogueDescription 내 액션 전용")]
    public sealed class CameraToPrincipleAction : YOLOActionBase
    {
        public override void Process(EntityData<IEntityData> e)
        {
            ActorProviderAttribute provider = e.GetAttribute<ActorProviderAttribute>();
            CameraManager.Instance.SetCameraTarget(new CameraManager.CameraTarget
            {
                m_Target = new CameraManager.TransformWrapper(provider.m_ActorProvider.Transform)
            });
        }
    }
}