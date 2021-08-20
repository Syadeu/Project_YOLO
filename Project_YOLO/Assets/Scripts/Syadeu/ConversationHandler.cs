using Syadeu.Database;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using System;

namespace Syadeu
{
    public sealed class ConversationHandler
    {
        private DialogueReference m_Dialogue;
        private int m_CurrentIndex = 0;
        private EntityData<YOLOActorEntity>[] m_JoinedEntities = Array.Empty<EntityData<YOLOActorEntity>>();
        private Action<EntityData<YOLOActorEntity>, string> m_OnConversation;

        public bool Started { get; private set; } = false;
        public EntityData<YOLOActorEntity> CurrentSpeaker { get; private set; }
        public string CurrentText { get; private set; }

        [Obsolete("Intended for pool container")]
        public ConversationHandler() { }

        public void Initialize(DialogueReference dialogue, params EntityData<YOLOActorEntity>[] joinedEntities)
        {
            m_Dialogue = dialogue;
            m_JoinedEntities = joinedEntities;
        }

        public void StartConversation(Action<EntityData<YOLOActorEntity>, string> onConversation, out float delay)
        {
            delay = 0;
            if (m_Dialogue.m_Texts.Length == 0)
            {
                Terminate();
                "대화 내용이 없네?".ToLog();
                return;
            }
            m_OnConversation = onConversation;

            //SetSpeaker(0);

            //m_OnConversation.Invoke(CurrentSpeaker, CurrentText);
            if (!MoveNext(out delay))
            {
                return;
            }
            //if (m_Dialogue.m_Texts[0].EnableAuto)
            //{
            //    CoreSystem.WaitInvoke(m_Dialogue.m_Texts[0].Delay, InternalMoveNext);
            //}

            Started = true;
        }
        //private void InternalMoveNext() => MoveNext();
        private EntityData<YOLOActorEntity> FindSpeaker(Reference<YOLOActorEntity> reference)
        {
            for (int i = 0; i < m_JoinedEntities.Length; i++)
            {
                if (m_JoinedEntities[i].Hash.Equals(reference.m_Hash))
                {
                    return m_JoinedEntities[i];
                }
            }
            return EntityData<YOLOActorEntity>.Empty;
        }
        private void SetSpeaker(int idx, out float delay)
        {
            delay = 0;
            if (idx >= m_Dialogue.m_Texts.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(SetSpeaker));
            }

            DialogueReference.Text currentText = m_Dialogue.m_Texts[idx];

            CurrentSpeaker = FindSpeaker(currentText.Principle);
            //CoreSystem.Logger.False(CurrentSpeaker.Equals(EntityData<YOLOActorEntity>.Empty), nameof(StartConversation));
            if (CurrentSpeaker.Equals(EntityData<YOLOActorEntity>.Empty))
            {
                $"{idx}번째 대화 스킵, 대화 상대를 찾을 수 없음".ToLog();
                CurrentText = string.Empty;
                return;
            }
            if (currentText.Messages.Length == 0)
            {
                $"{idx}번째 대화 스킵, 텍스트가 없음".ToLog();
                CurrentSpeaker = EntityData<YOLOActorEntity>.Empty;
                CurrentText = string.Empty;
                return;
            }

            int textIdx = UnityEngine.Random.Range(0, currentText.Messages.Length);
            CurrentText = currentText.Messages[textIdx];
            delay = currentText.EnableAuto ? currentText.Delay : 0;

            for (int i = 0; i < currentText.Actions?.Length; i++)
            {
                currentText.Actions[i].GetObject().Process(EntityData<IEntityData>.GetEntityData(CurrentSpeaker.Idx));
            }
        }

        private void Terminate()
        {
            for (int i = 0; i < m_Dialogue.OnEndofDialogueActions?.Length; i++)
            {
                m_Dialogue.OnEndofDialogueActions[i].GetObject().Process(CurrentSpeaker);
            }

            Started = false;
            m_Dialogue = null;
            CurrentText = string.Empty;
            m_CurrentIndex = 0;
            m_JoinedEntities = Array.Empty<EntityData<YOLOActorEntity>>();
            m_OnConversation = null;
            PoolContainer<ConversationHandler>.Enqueue(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>아직 대화중인가요?</returns>
        public bool MoveNext(out float delay)
        {
            delay = 0;
            if (m_Dialogue == null) return false;

            if (m_CurrentIndex.Equals(m_Dialogue.m_Texts.Length))
            {
                Terminate();
                return false;
            }

            SetSpeaker(m_CurrentIndex, out delay);
            if (CurrentSpeaker.Equals(EntityData<YOLOActorEntity>.Empty))
            {
                m_CurrentIndex++;
                return MoveNext(out delay);
            }
            m_OnConversation.Invoke(CurrentSpeaker, CurrentText);

            m_CurrentIndex++;
            return true;
        }
        //
    }
}