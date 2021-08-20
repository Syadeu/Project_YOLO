using Newtonsoft.Json;
using Syadeu.Database;
using Syadeu.Presentation;
using Syadeu.Presentation.Attributes;
using Syadeu.Presentation.Entities;
using System;
using System.Collections.Generic;

namespace Syadeu
{
    [AttributeAcceptOnly(typeof(YOLOActorEntity))]
    public sealed class DialogueAttribute : AttributeBase
    {
        [JsonProperty(Order = 0, PropertyName = "Dialogues")]
        public Reference<DialogueReference>[] m_Dialogues = Array.Empty<Reference<DialogueReference>>();

        [JsonIgnore] internal readonly Dictionary<Hash, DialogueReference> m_CachedDialogues = new Dictionary<Hash, DialogueReference>();

        public bool HasDialogue(DialogueID id) => m_CachedDialogues.ContainsKey(id.AttributeHash);
        public DialogueReference GetDialogue(DialogueID id)
        {
            if (!m_CachedDialogues.TryGetValue(id.AttributeHash, out DialogueReference value))
            {
                return null;
            }

            value.Initialize();
            return value;
        }

        public bool TryConversation(DialogueID id, out ConversationHandler handler, params EntityData<YOLOActorEntity>[] joinedEntities)
        {
            handler = null;
            if (id.AttributeHash.Equals(0))
            {
                "ID Hash 가 지정되지 않은 DialogueID".ToLog(id);
                return false;
            }

            DialogueReference dialogue = GetDialogue(id);
            if (dialogue == null || dialogue.m_Texts.Length == 0)
            {
                "아무 대화도 없음".ToLog();
                return false;
            }
            else if (!dialogue.m_Texts[0].Principle.m_Hash.Equals(Parent.Target.Hash))
            {
                "대화 주체가 아님".ToLog();
                return false;
            }

            handler = PoolContainer<ConversationHandler>.Dequeue();
            List<EntityData<YOLOActorEntity>> temp = new List<EntityData<YOLOActorEntity>>();

            EntityData<YOLOActorEntity> convert = EntityData<YOLOActorEntity>.GetEntityData(Parent.Idx);
            temp.Add(convert);
            temp.AddRange(joinedEntities);

            handler.Initialize(dialogue, temp.ToArray());
            return true;
        }
    }

    internal sealed class DialogueProcessor : AttributeProcessor<DialogueAttribute>
    {
        protected override void OnCreated(DialogueAttribute attribute, EntityData<IEntityData> entity)
        {
            for (int i = 0; i < attribute.m_Dialogues.Length; i++)
            {
                if (attribute.m_CachedDialogues.ContainsKey(attribute.m_Dialogues[i].m_Hash)) continue;

                attribute.m_CachedDialogues.Add(attribute.m_Dialogues[i].m_Hash, attribute.m_Dialogues[i].GetObject());
            }
        }
    }
}