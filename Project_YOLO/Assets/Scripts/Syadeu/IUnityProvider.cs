using UnityEngine;

namespace Syadeu
{
    public interface IUnityProvider
    {
        string name { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}