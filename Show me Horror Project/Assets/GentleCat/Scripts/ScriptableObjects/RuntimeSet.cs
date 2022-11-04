using System.Collections.Generic;
using UnityEngine;

namespace GentleCat.ScriptableObjects
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        [SerializeField] private List<T> defaultItems = new List<T>();
        public List<T> Items { get; set; } = new List<T>();

        private void OnEnable()
        {
            Items.AddRange(defaultItems);
        }

        

        public void Add(T thing)
        {
            if (!Items.Contains(thing))
                Items.Add(thing);
        }

        public void Remove(T thing)
        {
            if (Items.Contains(thing))
                Items.Remove(thing);
        }

        public List<T> GetDefault()
        {
            return defaultItems;
        }
    }
}