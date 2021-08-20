using Newtonsoft.Json;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    public sealed class CameraToCameraTargetAction : YOLOActionBase
    {
        [JsonProperty] private string m_CameraTargetName = string.Empty;

        public override void Process(EntityData<IEntityData> entity)
        {
            CameraManager.Instance.SetCameraTarget(m_CameraTargetName);
        }
    }
}