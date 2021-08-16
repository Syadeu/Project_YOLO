namespace Syadeu
{
    public interface IItem : IUnityProvider
    {
        ItemType ItemType { get; }
    }

    public enum ItemType
    {
        None        =   0,
        Seed        =   1,
        Blueprint   =   2
    }
}