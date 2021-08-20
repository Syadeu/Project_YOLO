using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using System;
using System.Collections;
using UnityEngine;

namespace Syadeu
{
    public sealed class TestNPC : MonoBehaviour, IActor
    {
        public ActorID m_ActorID;
        public DialogueID m_DialogueID;
        public PlayerController PlayerController;

        public Animator Animator => throw new System.NotImplementedException();
        public ActorID ActorID => m_ActorID;
        public SkillDescription[] Skills => Array.Empty<SkillDescription>();

        private ActorProvider<TestNPC> ActorProvider;

        private IEnumerator Start()
        {
            CoreSystem.WaitInvoke(PresentationSystem<YOLO_ActorSystem>.IsValid, RegisterActor);
            yield return new WaitForSeconds(2);

            while (!PlayerController.ActorProvider.IsInitialized)
            {
                //$"{PlayerController.ActorProvider.Entity.Name}:{PlayerController.ActorProvider.Entity.IsValid()}: {PlayerController.ActorProvider.IsInitialized}".ToLog();
                yield return null;
            }

            ActorProvider.TryConversation(m_DialogueID, PlayerController.ActorProvider, out var handler);

            handler.StartConversation(Conversation);
            yield return null;

            while (handler.MoveNext())
            {
                yield return new WaitForSeconds(1);
            }
        }

        private void Conversation(EntityData<YOLOActorEntity> entity, string text)
        {
            $"{entity.Name} 이 {text} 를 말함".ToLog();
        }

        private void RegisterActor()
        {
            ActorProvider = PresentationSystem<YOLO_ActorSystem>.System.RegisterActor(this);
        }
    }
}