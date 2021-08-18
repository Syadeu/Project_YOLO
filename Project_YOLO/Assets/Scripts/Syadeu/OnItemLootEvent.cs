using Syadeu.Presentation;

namespace Syadeu
{
    /// <summary>
    /// 아이템을 얻으면 발생하는 이벤트입니다.
    /// </summary>
    public sealed class OnItemLootEvent : SynchronizedEvent<OnItemLootEvent>
    {
        /// <summary>
        /// 아이템을 주운 캐릭터
        /// </summary>
        public ActorProviderBase Character { get; private set; }
        /// <summary>
        /// 어떤 아이템을 주웠는지
        /// </summary>
        public IItem Item { get; private set; }

        public static OnItemLootEvent GetEvent(ActorProviderBase actor, IItem item)
        {
            OnItemLootEvent temp = Dequeue();
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