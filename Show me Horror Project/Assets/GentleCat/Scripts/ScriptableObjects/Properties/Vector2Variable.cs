using System;
using UnityEngine;

namespace GentleCat.ScriptableObjects.Properties
{
    [CreateAssetMenu(menuName = "Tools/Variables/Vector2 Variable"), Serializable]
    public class Vector2Variable : ObjectVariable<Vector2>
    {
    }

    [Serializable]
    public class Vector2Reference : ObjectReference<Vector2, Vector2Variable>
    {
    }
}