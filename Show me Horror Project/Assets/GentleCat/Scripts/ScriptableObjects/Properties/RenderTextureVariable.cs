using System;
using UnityEngine;

namespace GentleCat.ScriptableObjects.Properties
{
    [CreateAssetMenu(menuName = "Tools/Variables/RenderTexture Variable"), Serializable]
    public class RenderTextureVariable : ObjectVariable<RenderTexture>
    {
    }

    [Serializable]
    public class RenderTextureReference : ObjectReference<RenderTexture, RenderTextureVariable>
    {
    }
}