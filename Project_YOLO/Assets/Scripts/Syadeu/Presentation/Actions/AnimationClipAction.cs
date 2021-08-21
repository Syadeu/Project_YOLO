using Newtonsoft.Json;
using Syadeu.Database;
using Syadeu.Presentation;
using Syadeu.Presentation.Entities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Syadeu
{
    public sealed class AnimationClipAction : YOLOActionBase
    {
        [JsonProperty] public Reference<ObjectEntity> m_Entity;
        [JsonProperty] public PrefabReference m_AnimationClip;

        [JsonProperty] public Reference<YOLOActionBase>[] m_OnClipStartActions = Array.Empty<Reference<YOLOActionBase>>();
        [JsonProperty] public Reference<YOLOActionBase>[] m_OnClipEndActions = Array.Empty< Reference<YOLOActionBase>>();

        public override void Process(EntityData<IEntityData> entity)
        {
            var oper = Addressables.LoadAssetAsync<AnimationClip>(m_AnimationClip.GetObjectSetting().m_RefPrefab);
            oper.Completed += Oper_Completed;

            void Oper_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AnimationClip> obj)
            {
                Vector3 pos = entity.GetAttribute<ActorProviderAttribute>().m_ActorProvider.Transform.position;


                Entity<IEntity> targetEntity = PresentationSystem<EntitySystem>.System.CreateEntity(m_Entity, pos);
                AnimationClip clip = obj.Result;

                CoreSystem.StartUnityUpdate(this, Update(targetEntity, clip));
            }
        }

        private IEnumerator Update(Entity<IEntity> entity, AnimationClip clip)
        {
            while (!entity.hasProxy)
            {
                yield return null;
            }

            ProxyTransform tr = (ProxyTransform)entity.transform;
            Animation anim = tr.proxy.GetComponent<Animation>();
            if (anim == null) anim = tr.proxy.AddComponent<Animation>();

            anim.clip = clip;
            anim.Play();
            "play".ToLog();
            for (int i = 0; i < m_OnClipStartActions.Length; i++)
            {
                if (!m_OnClipStartActions[i].IsValid())
                {
                    $"대화의 {i} 번째 액션이 유효하지 않음".ToLogError();
                    continue;
                }

                m_OnClipStartActions[i].GetObject().Process(entity.Idx);
            }

            float timer = 0;
            while (timer < anim.clip.length)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            "exit".ToLog();
            for (int i = 0; i < m_OnClipEndActions.Length; i++)
            {
                if (!m_OnClipEndActions[i].IsValid())
                {
                    $"대화의 {i} 번째 액션이 유효하지 않음".ToLogError();
                    continue;
                }

                m_OnClipEndActions[i].GetObject().Process(entity.Idx);
            }
        }
    }
}