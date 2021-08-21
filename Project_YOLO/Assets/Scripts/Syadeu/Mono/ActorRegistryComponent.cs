using System;
using System.Collections;
using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(Animator))]
    public sealed class ActorRegistryComponent : MonoBehaviour, IActor
    {
        private Animator m_Animator;
        private ActorProvider<ActorRegistryComponent> m_ActorProvider;

        public ActorID m_ActorID;

        public Animator Animator => m_Animator;
        public ActorID ActorID => m_ActorID;
        public SkillDescription[] Skills => Array.Empty<SkillDescription>();

        public ActorProvider<ActorRegistryComponent> ActorProvider => m_ActorProvider;

        private void OnEnable()
        {
            m_Animator = GetComponent<Animator>();
        }
        private IEnumerator Start()
        {
            if (m_ActorID == null)
            {
                $"{name}은 액터아이디가 없음!".ToLogError();
                yield break;
            }

            yield return new WaitUntil(() => YOLOPresentationProvider.Instance.ActorSystem != null);
            m_ActorProvider = YOLOPresentationProvider.Instance.ActorSystem.RegisterActor(this);
        }
    }
}
