using System;
using System.Collections;
using UnityEngine;

namespace Syadeu
{
    public abstract class CLRSingleTone<T> : IDisposable where T : CLRSingleTone<T>, new()
    {
        private static T s_Instance;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new T();
                    s_Instance.OnInitialize();
                }
                return s_Instance;
            }
        }
        ~CLRSingleTone()
        {
            Dispose();
        }

        protected virtual void OnInitialize() { }
        public virtual void Dispose() { }
    }
}