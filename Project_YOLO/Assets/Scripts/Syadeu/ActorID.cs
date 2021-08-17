using Syadeu.Database;
using System;
using UnityEngine;

namespace Syadeu
{
    [CreateAssetMenu(fileName = "NewActorID", menuName = "Actor/ID")]
    public sealed class ActorID : ScriptableObject, IEquatable<ActorID>
    {
        [SerializeField] private ActorType m_ActorType;
        [SerializeField] private float m_HP;

        public Hash ID { get; private set; }
        public ActorType ActorType => m_ActorType;
        public float HP => m_HP;

        private void OnEnable()
        {
            ID = Hash.NewHash();
        }

        public bool Equals(ActorID other) => ID.Equals(other.ID);
    }
}