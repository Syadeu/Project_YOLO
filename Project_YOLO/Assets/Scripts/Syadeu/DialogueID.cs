using Syadeu.Database;
using System;
using UnityEngine;

namespace Syadeu
{
    [CreateAssetMenu(fileName = "NewDialogueID", menuName = "Actor/DialogueID")]
    public sealed class DialogueID : ScriptableObject, IEquatable<DialogueID>
    {
        [SerializeField] private ulong m_AttributeHash;

        public Hash AttributeHash => m_AttributeHash;

        public bool Equals(DialogueID other) => m_AttributeHash.Equals(other.m_AttributeHash);
    }
}