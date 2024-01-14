using UnityEngine;
using UnityEngine.UI;

namespace UIFramework.Utils
{
    /// <summary>
    /// this creates an empty image without drawcalls, replaces images with alpha set to zero as raycast targets
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class RaycastHelper : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}