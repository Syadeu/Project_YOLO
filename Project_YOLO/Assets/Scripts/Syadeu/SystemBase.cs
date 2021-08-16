using Syadeu.Internal;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Syadeu
{
    [RequireDerived]
    public abstract class SystemBase
    {
        private bool m_Initialized = false;
        internal Queue<(Type, Action<SystemBase>)> m_Requests = new Queue<(Type, Action<SystemBase>)>();

        internal void InternalOnInitialize()
        {
            OnInitialize();
            m_Initialized = true;
        }

        protected virtual void OnInitialize() { }
        protected virtual void RequestSystem<T>(Action<T> setter) where T : SystemBase
        {
            if (m_Initialized)
            {
                setter.Invoke(SystemInitializer.Instance.GetSystem<T>());
                return;
            }

            m_Requests.Enqueue((TypeHelper.TypeOf<T>.Type, (other) => setter.Invoke((T)other)));
        }
    }
    public abstract class System<T> : SystemBase where T : System<T>
    {
        public static T Instance => SystemInitializer.Instance.GetSystem<T>();
    }
}