using Syadeu.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Syadeu
{
    public sealed class SystemInitializer : CLRSingleTone<SystemInitializer>
    {
        private readonly Dictionary<Type, SystemBase> m_Systems = new Dictionary<Type, SystemBase>();
        private SystemBase[] m_RegisteredSystems;

        protected override void OnInitialize()
        {
            Type[] types = TypeHelper.GetTypes((other) => TypeHelper.TypeOf<SystemBase>.Type.IsAssignableFrom(TypeHelper.TypeOf<SystemBase>.Type));

            m_RegisteredSystems = new SystemBase[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                ConstructorInfo ctor = TypeHelper.GetConstructorInfo(types[i]);
                SystemBase system = (SystemBase)ctor.Invoke(null, null);

                m_RegisteredSystems[i] = system;
                m_Systems.Add(system.GetType(), system);
            }

            for (int i = 0; i < m_RegisteredSystems.Length; i++)
            {
                int requestCount = m_RegisteredSystems[i].m_Requests.Count;
                for (int a = 0; a < requestCount; a++)
                {
                    (Type, Action<SystemBase>) temp = m_RegisteredSystems[i].m_Requests.Dequeue();
                    temp.Item2.Invoke(m_Systems[temp.Item1]);
                }
            }

            for (int i = 0; i < m_RegisteredSystems.Length; i++)
            {
                m_RegisteredSystems[i].InternalOnInitialize();
            }
        }

        internal T GetSystem<T>() where T : SystemBase
        {
            if (m_Systems.TryGetValue(TypeHelper.TypeOf<T>.Type, out var system))
            {
                return (T)system;
            }
            return null;
        }
    }
}