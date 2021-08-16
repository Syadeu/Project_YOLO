using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Syadeu
{
    public sealed class InventoryProvider<T> where T : MonoBehaviour, IActor
    {
        static Vector3 c_InitPosition = new Vector3(9999, -9999, 9999);

        private readonly T m_Actor;
        private readonly List<IItem> m_Items = new List<IItem>();

        public InventoryProvider(T actor)
        {
            m_Actor = actor;
        }

        public void InsertItem(IItem item)
        {
            m_Items.Add(item);

            item.gameObject.SetActive(false);
            item.transform.position = c_InitPosition;
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