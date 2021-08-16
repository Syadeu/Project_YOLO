using System;
using UnityEngine;

namespace Syadeu
{
    public sealed class StatProvider<T> where T : MonoBehaviour, IActor
    {
        private readonly T m_Actor;

        private float m_HP;

        public event Action<float> OnHPChanged;

        public bool IsDead => m_HP <= 0;
        public float HP
        {
            get => m_HP;
            set
            {
                m_HP = value;
                OnHPChanged?.Invoke(value);
                // TODO : observer 달기
            }
        }

        public StatProvider(T actor)
        {
            m_Actor = actor;
        }
    }
}