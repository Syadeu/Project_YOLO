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

        //private IEnumerator Start()
        //{
        //    CoreSystem.WaitInvoke(() => YOLOPresentationProvider.Instance.ActorSystem != null, RegisterActor);
        //    yield return new WaitForSeconds(2);

        //    while (PlayerController.ActorProvider == null ||
        //        !PlayerController.ActorProvider.IsInitialized)
        //    {
        //        //$"{PlayerController.ActorProvider.Entity.Name}:{PlayerController.ActorProvider.Entity.IsValid()}: {PlayerController.ActorProvider.IsInitialized}".ToLog();
        //        yield return null;
        //    }

        //    YOLOPresentationProvider.Instance.GameSystem.StartConversation(m_DialogueID, PlayerController.ActorProvider.Entity, ActorProvider.Entity);

        //    //ActorProvider.TryConversation(m_DialogueID, out var handler, PlayerController.ActorProvider.Entity);

        //    //handler.StartConversation(Conversation);
        //    //yield return null;

        //    //while (handler.MoveNext())
        //    //{
        //    //    yield return new WaitForSeconds(1);
        //    //}
        //}

        private void Conversation(EntityData<YOLOActorEntity> entity, string text)
        {
            $"{entity.Name} 이 {text} 를 말함".ToLog();
        }

        private void RegisterActor()
        {
            ActorProvider = YOLOPresentationProvider.Instance.ActorSystem.RegisterActor(this);
        }
    }
}