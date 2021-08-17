using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Syadeu
{
    public sealed class InventoryProvider<T> : IDisposable where T : MonoBehaviour, IActor
    {
        private T m_Actor;
        private List<IItem> m_Items = new List<IItem>();

        public InventoryProvider(T actor)
        {
            m_Actor = actor;
        }
        ~InventoryProvider() => Dispose();
        public void Dispose()
        {
            m_Items.Clear();

            m_Actor = null;
            m_Items = null;
        }

        public void InsertItem(IItem item)
        {
            m_Items.Add(item);
        }
        public IItem FindAndExtract(ItemType itemType)
        {
            var iter = m_Items.Where((other) => other.ItemType.Equals(itemType));
            if (iter.Any())
            {
                IItem item = iter.First();
                m_Items.Remove(item);
                return item;
            }

            return null;
        }
    }
}