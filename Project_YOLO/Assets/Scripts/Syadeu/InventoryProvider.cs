using Syadeu.Presentation;
using Syadeu.Presentation.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Syadeu
{
    public sealed class InventoryProvider<T> : ProviderBase, IDisposable where T : MonoBehaviour, IActor
    {
        private T m_Actor;
        private ActorProvider<T> m_ActorProvider;
        private List<IItem> m_Items = new List<IItem>();

        public InventoryProvider(T actor, ActorProvider<T> actorProvider)
        {
            m_Actor = actor;
            m_ActorProvider = actorProvider;
        }
        ~InventoryProvider() => Dispose();
        public void Dispose()
        {
            m_Items.Clear();

            m_Actor = null;
            m_ActorProvider = null;
            m_Items = null;
        }

        public void InsertItem(IItem item)
        {
            m_Items.Add(item);
            PresentationSystem<EventSystem>.System.PostEvent(OnItemLootEvent.GetEvent(m_ActorProvider, item));
        }
        public IItem FindAndExtract(ItemType itemType)
        {
            var iter = m_Items.Where((other) => other.ItemType.Equals(itemType));
            if (iter.Any())
            {
                IItem item = iter.First();
                m_Items.Remove(item);

                PresentationSystem<EventSystem>.System.PostEvent(OnItemDropEvent.GetEvent(m_ActorProvider, item));
                return item;
            }

            return null;
        }
    }
}