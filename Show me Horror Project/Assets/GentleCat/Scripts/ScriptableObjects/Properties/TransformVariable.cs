using System;
using UnityEngine;

namespace GentleCat.ScriptableObjects.Properties
{
    [CreateAssetMenu(menuName = "Tools/Variables/Transform Variable"), Serializable]
    public class TransformVariable : ObjectVariable<Transform>
    {
    }
}