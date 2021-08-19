using System;
using UnityEngine;

namespace Syadeu
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialouge/Description")]
    public sealed class DialogueDescription : ScriptableObject
    {
        [Serializable]
        public sealed class Log
        {
            public ActorID From;
            public string[] m_Description;
        }

        public Log[] m_Logs = Array.Empty<Log>();
    }
}