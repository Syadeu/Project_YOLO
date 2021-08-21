using Syadeu.Presentation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Syadeu.Mono
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraManager : MonoManager<CameraManager>
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private float m_CameraSpeed = 2;
        [SerializeField] private Vector3 m_Origin = new Vector3(2.3f, -22.2f, -10);

        [SerializeField] private CameraTarget[] m_RoomTargets = Array.Empty<CameraTarget>();

        #region Inner Class

        [Serializable]
        public sealed class CameraTarget
        {
            public string Name = "NewTarget";
            public TransformWrapper m_Target = null;
            public Vector3 m_Offset = Vector3.zero;
            public float m_Zoom = 13;
        }

        [Serializable]
        public sealed class TransformWrapper
        {
            [SerializeField] private Transform tr;
            [NonSerialized] private ITransform Tr;

            public Vector3 Position
            {
                get
                {
                    if (tr == null) return Tr.position;
                    return tr.position;
                }
            }

            public TransformWrapper(Transform tr)
            {
                this.tr = tr;
            }
            public TransformWrapper(ITransform tr)
            {
                this.Tr = tr;
            }
        }

        #endregion

        private readonly Dictionary<string, CameraTarget> m_CachedRoomTargets = new Dictionary<string, CameraTarget>();

        private CameraTarget m_CamTarget;

        public Camera Camera => m_Camera;
        public CameraTarget CurrentTarget => m_CamTarget;

        public override void OnInitialize()
        {
            if (m_Camera == null) m_Camera = GetComponent<Camera>();

            transform.position = m_Origin;

            for (int i = 0; i < m_RoomTargets.Length; i++)
            {
                m_CachedRoomTargets.Add(m_RoomTargets[i].Name, m_RoomTargets[i]);
            }

            AddConsoleCommand();
            if (m_RoomTargets.Length == 0)
            {
                "룸타겟이 하나도 없네".ToLogError();
            }

            StartUnityUpdate(CameraUpdate());
        }
        private void AddConsoleCommand()
        {
            ConsoleWindow.CreateCommand((cmd) =>
            {
                if (string.IsNullOrEmpty(cmd) ||
                    !int.TryParse(cmd, out int idx))
                {
                    ConsoleWindow.Log("잘못된 인풋");
                    return;
                }

                SetCameraTarget(m_RoomTargets[idx]);

            }, "camera", "move");
        }
        private IEnumerator CameraUpdate()
        {
            while (true)
            {
                UpdateCamera();

                yield return null;
            }

            void UpdateCamera()
            {
                if (m_CamTarget == null) return;

                Vector3 targetPos = m_CamTarget.m_Target.Position;
                targetPos += m_CamTarget.m_Offset;
                //targetPos.z = m_Origin.z;

                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * m_CameraSpeed);

                //m_Camera.orthographicSize = Mathf.Lerp(m_Camera.orthographicSize, m_CamTarget.m_Zoom, Time.deltaTime * m_CameraSpeed);
            }
        }

        public void SetCameraTarget(CameraTarget tr)
        {
            m_CamTarget = tr;
        }
        public void SetCameraTarget(string targetName)
        {
            if (!m_CachedRoomTargets.TryGetValue(targetName, out var target))
            {
                $"{targetName} 은 카메라에 존재하지 않는 타겟!".ToLogError();
                return;
            }
            m_CamTarget = target;
        }
    }
}
