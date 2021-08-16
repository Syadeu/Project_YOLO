namespace Syadeu
{
    public interface IItem : IUnityProvider
    {
        ItemType ItemType { get; }
    }
}