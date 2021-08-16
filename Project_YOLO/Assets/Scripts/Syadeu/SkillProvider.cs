using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syadeu
{
    public sealed class SkillProvider<T> where T : MonoBehaviour, IActor
    {
        private readonly T m_Actor;
        private readonly Dictionary<string, SkillDescription> m_Skills = new Dictionary<string, SkillDescription>();

        public SkillProvider(T actor)
        {
            m_Actor = actor;

            for (int i = 0; i < actor.Skills.Length; i++)
            {
                m_Skills.Add(actor.Skills[i].name, actor.Skills[i]);
            }
        }

        public void PlaySkill(string skillName)
        {
            SkillDescription skill = m_Skills[skillName];
            m_Actor.Animator.SetTrigger(skill.AnimatorHash);

            for (int i = 0; i < skill.Effects.Length; i++)
            {
                if (skill.Effects[i].EffectObject == null)
                {
                    Debug.LogError($"캐릭터({m_Actor.name}) 스킬 {skill.name}의 {i}번째 이펙트가 지정되지않음");
                    continue;
                }
                m_Actor.StartCoroutine(StartEffect(skill.Effects[i]));
            }

            IEnumerator StartEffect(SkillDescription.Effect effect)
            {
                yield return new WaitForSeconds(effect.StartDelay);

                GameObject fx = Object.Instantiate(effect.EffectObject, effect.LocalPosition, effect.LocalRotation, m_Actor.transform);

                yield return new WaitForSeconds(effect.Duration);

                Object.Destroy(fx);
            }
        }
    }
}