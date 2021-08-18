using Syadeu.Presentation;

namespace Syadeu
{
    /// <summary>
    /// 아이템을 떨구면(<see cref="InventoryProvider{T}.FindAndExtract(ItemType)"/>) 발생하는 이벤트입니다.
    /// </summary>
    /// <remarks>
    /// 이 이벤트가 실행된 시점에는 해당 캐릭터의 인벤토리엔 여기에 반환된 아이템(<see cref="Item"/>)을 들고있지 않습니다.
    /// </remarks>
    public sealed class OnItemDropEvent : SynchronizedEvent<OnItemDropEvent>
    {
        /// <summary>
        /// 아이템을 떨군 캐릭터
        /// </summary>
        public ActorProviderBase Character { get; private set; }
        /// <summary>
        /// 어떤 아이템을 떨궜는지
        /// </summary>
        public IItem Item { get; private set; }

        public static OnItemDropEvent GetEvent(ActorProviderBase actor, IItem item)
        {
            var temp = Dequeue();
            temp.Character = actor;
            temp.Item = item;
            return temp;
        }
        protected override void OnTerminate()
        {
            Character = null;
            Item = null;
        }
    }
}