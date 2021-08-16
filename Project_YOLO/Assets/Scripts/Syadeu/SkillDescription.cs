using System;
using UnityEngine;

namespace Syadeu
{
    [CreateAssetMenu(fileName = "NewSkillDescription", menuName = "Actor/Skill Description")]
    public sealed class SkillDescription : ScriptableObject
    {
        [Serializable]
        public sealed class Effect
        {
            [SerializeField] private string Name;

            [Space]
            public GameObject EffectObject;
            public Vector3 LocalPosition;
            public Quaternion LocalRotation = Quaternion.identity;

            [Space]
            public float StartDelay = 0;
            public float Duration = 3;
        }

        [SerializeField] private string m_AnimationTrigger;
        [SerializeField] private Effect[] m_Effects = Array.Empty<Effect>();

        private int m_AnimatorHash;

        public int AnimatorHash => m_AnimatorHash;
        public Effect[] Effects => m_Effects;

        private void OnEnable()
        {
            m_AnimatorHash = Animator.StringToHash(m_AnimationTrigger);
        }
    }
}