using Syadeu.Mono;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(ObjectEntity))]
    public sealed class UIManagerAttribute : AttributeBase
    {

    }
    internal sealed class UIManagerProcessor : AttributeProcessor<UIManagerAttribute>,
        IAttributeOnProxyCreated
    {
        public void OnProxyCreated(AttributeBase attribute, Entity<IEntity> entity, RecycleableMonobehaviour monoObj)
        {
            UIManager uIManager = monoObj.GetComponent<UIManager>();

            CoreSystem.WaitInvoke(() => CameraManager.HasInstance, () =>
            {
                uIManager.WorldCanvas.worldCamera = CameraManager.Instance.Camera;
            });
            
        }
    }
}