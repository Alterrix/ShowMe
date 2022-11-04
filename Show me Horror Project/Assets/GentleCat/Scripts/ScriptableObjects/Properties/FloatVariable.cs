using System;
using UnityEngine;

namespace GentleCat.ScriptableObjects.Properties
{
    [CreateAssetMenu(menuName =  "Tools/Variables/Float Variable"), Serializable]
    public class FloatVariable : ObjectVariable<float>
    {
        public static string folder = "Folder";
    }

    [Serializable]
    public class FloatReference : ObjectReference<float, FloatVariable>
    {
    }
}