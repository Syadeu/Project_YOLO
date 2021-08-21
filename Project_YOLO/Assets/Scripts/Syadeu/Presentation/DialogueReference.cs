using Newtonsoft.Json;
using Syadeu.Database;
using Syadeu.Internal;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Syadeu
{
    [AttributeAcceptOnly(null)]
    public sealed class DialogueReference : AttributeBase
    {
        [Flags]
        public enum Culture
        {
            None    =   0,
            Korean  =   0b001,
            English =   0b010,

            All     =   ~0
        }

        [Serializable]
        public sealed class Text
        {
            [ReflectionDescription("말하는 주체")]
            public Reference<YOLOActorEntity> Principle;
            public string[] Messages = Array.Empty<string>();

            [Space]
            public Reference<YOLOActionBase>[] Actions = Array.Empty<Reference<YOLOActionBase>>();

            [Space]
            [ReflectionDescription("활성화시 Delay 만큼 기다린후 다음 대화로 자동으로 넘어감")]
            public bool EnableAuto = false;
            [ReflectionDescription("말하고나서 기다릴 시간(초)")]
            public float Delay = 0;
        }
        [JsonProperty] public Reference<YOLOActionBase>[] OnStartDialogueActions = Array.Empty<Reference<YOLOActionBase>>();
        [JsonProperty] public Reference<YOLOActionBase>[] OnEndofDialogueActions = Array.Empty<Reference<YOLOActionBase>>();

        [ReflectionDescription("0번째 인덱스는 무조건 대화를 시작하는 주체입니다")]
        [JsonProperty(PropertyName = "Texts")] public Text[] m_Texts = Array.Empty<Text>();
        [JsonProperty] public bool IsMoveable = false;

        [Space]
        [ReflectionDescription("아래 액션의 주체는 무조건 이 Dialogue 의 0번째 화자(즉 이 대화를 시작한)입니다.")]

        [JsonIgnore] private bool m_Initialized = false;
        [JsonIgnore] private readonly HashSet<Hash> m_JoinedEntities = new HashSet<Hash>();

        public void Initialize()
        {
            if (m_Initialized) return;

            for (int i = 0; i < m_Texts.Length; i++)
            {
                if (m_JoinedEntities.Contains(m_Texts[i].Principle.m_Hash)) continue;

                m_JoinedEntities.Add(m_Texts[i].Principle.m_Hash);
            }

            m_Initialized = true;
        }
        public bool HasEntity(Hash entityHash) => m_JoinedEntities.Contains(entityHash);
    }
}