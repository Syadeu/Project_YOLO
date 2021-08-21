using Newtonsoft.Json;
using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using Unity.Mathematics;

namespace Syadeu
{
    public sealed class CameraToTargetEntity : YOLOActionBase
    {
        [JsonProperty] public float3 Offset = 0;

        public override void Process(EntityData<IEntityData> entity)
        {
            if (!(entity.Target is EntityBase entityBase))
            {
                $"Object entity 가 아님".ToLogError();
                return;
            }

            CameraManager.Instance.SetCameraTarget(new CameraManager.CameraTarget
            {
                m_Target = new CameraManager.TransformWrapper(entityBase.transform),
                m_Offset = Offset
            });
        }
    }
}