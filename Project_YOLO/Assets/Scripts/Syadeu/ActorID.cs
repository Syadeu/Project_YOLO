using Syadeu.Database;
using System;
using UnityEngine;

namespace Syadeu
{
    [CreateAssetMenu(fileName = "NewActorID", menuName = "Actor/ID")]
    public sealed class ActorID : ScriptableObject, IEquatable<ActorID>
    {
        [SerializeField] private ulong m_EntityHash;

        public Hash EntityHash => m_EntityHash;

        public bool Equals(ActorID other) => m_EntityHash.Equals(other.m_EntityHash);
    }
}