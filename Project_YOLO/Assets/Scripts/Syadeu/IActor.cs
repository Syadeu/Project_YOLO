using UnityEngine;

namespace Syadeu
{
    public interface IActor : IUnityProvider
    {
        Animator Animator { get; }

        ActorID ActorID { get; }
        SkillDescription[] Skills { get; }
    }
}