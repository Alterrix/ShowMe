using System;
using UnityEngine;

namespace GentleCat.ScriptableObjects.Properties
{
    [CreateAssetMenu(menuName = "Tools/Variables/Int Variable"), Serializable]
    public class IntVariable : ObjectVariable<int>
    {
    }

    [Serializable]
    public class IntReference : ObjectReference<int, IntVariable>
    {
    }
}