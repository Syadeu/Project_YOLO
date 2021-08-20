using Syadeu.Presentation;
using Syadeu.Presentation.Events;

namespace Syadeu
{
    [SubSystem(typeof(EventSystem))]
    public sealed class YOLO_GameSystem : PresentationSystemEntity<YOLO_GameSystem>
    {
        public override bool EnableBeforePresentation => false;
        public override bool EnableOnPresentation => false;
        public override bool EnableAfterPresentation => false;

        private EventSystem m_EventSystem;

        protected override PresentationResult OnInitialize()
        {
            RequestSystem<EventSystem>(Bind);

            return base.OnInitialize();
        }
        public override void OnDispose()
        {
            m_EventSystem.RemoveEvent<OnItemLootEvent>(OnItemLootEventHandler);
            m_EventSystem.RemoveEvent<OnItemDropEvent>(OnItemDropEventHandler);
        }

        #region Bind

        private void Bind(EventSystem other)
        {
            m_EventSystem = other;
            m_EventSystem.AddEvent<OnItemLootEvent>(OnItemLootEventHandler);
            m_EventSystem.AddEvent<OnItemDropEvent>(OnItemDropEventHandler);
        }
        private void OnItemLootEventHandler(OnItemLootEvent ev)
        {
            if ((ev.Character.Entity.Target.ActorType & ActorType.Player) == ActorType.Player)
            {
                if (ev.Item.ItemType == ItemType.Seed)
                {
                    "씨앗을 얻었다!".ToLog();
                }
            }
        }
        private void OnItemDropEventHandler(OnItemDropEvent ev)
        {
            if ((ev.Character.Entity.Target.ActorType & ActorType.Player) == ActorType.Player)
            {
                if (ev.Item.ItemType == ItemType.Seed)
                {
                    "씨앗을 떨궜다!".ToLog();
                }
            }
        }

        #endregion
    }
}