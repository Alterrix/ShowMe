using UnityEngine;
using UnityEngine.SceneManagement;

namespace GentleCat.ScriptableObjects
{
    public abstract class ObjectVariable<T> : ScriptableObject
    {
        
        [SerializeField] private T defaultValue;

        public T CurrentValue { get; set; }

        public void OnEnable()
        {
            CurrentValue = defaultValue;
        }

        public T GetDefault()
        {
            return defaultValue;
        }
    }
}