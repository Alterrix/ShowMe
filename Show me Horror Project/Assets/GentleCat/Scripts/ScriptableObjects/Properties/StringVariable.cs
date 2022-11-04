using System;
using UnityEngine;

namespace GentleCat.ScriptableObjects.Properties
{
    [CreateAssetMenu(menuName = "Tools/Variables/String Variable"), Serializable]
    public class StringVariable : ObjectVariable<string>
    {
    }

    [Serializable]
    public class StringReference : ObjectReference<string, StringVariable>
    {
    }
}