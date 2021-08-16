using UnityEngine;

namespace Syadeu
{
    public interface IActor : IUnityProvider
    {
        Animator Animator { get; }
        SkillDescription[] Skills { get; }
    }
}