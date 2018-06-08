#region Copyright © 2016-2018 RenGuiYou. All rights reserved.
//=====================================================
// NeatlyFrameWork v1.x
// Filename:    EmptyRaycast.cs
// Author:      RenGuiyou
// Feedback: 	mailto:750539605@qq.com
//=====================================================
#endregion

using UnityEngine.UI;

namespace Neatly.UI
{
    public class EmptyRaycast : MaskableGraphic
    {
        protected EmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}