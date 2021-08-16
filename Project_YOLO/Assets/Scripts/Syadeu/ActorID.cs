using UnityEngine;

namespace Syadeu
{
    [CreateAssetMenu(fileName = "NewActorID", menuName = "Actor/ID")]
    public sealed class ActorID : ScriptableObject
    {
        [SerializeField] private ActorType m_ActorType;

        public int ID { get; private set; }
        public ActorType ActorType => m_ActorType;

        private void OnEnable()
        {
            ID = GetHashCode();
        }
    }
}