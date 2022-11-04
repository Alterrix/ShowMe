using UnityEngine;

namespace GentleCat.ScriptableObjects
{
    public abstract class RuntimeArray<T> : ScriptableObject
    {
        public T[] Items = new T[0];

        public void Init(int size)
        {
            Items = new T[size];
        }
    }
}