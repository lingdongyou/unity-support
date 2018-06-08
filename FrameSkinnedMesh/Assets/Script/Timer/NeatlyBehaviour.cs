#region Copyright © 2016-2018 RenGuiYou. All rights reserved.
//=====================================================
// NeatlyFrameWork v1.x
// Filename:    NeatlyBehaviour.cs
// Author:      RenGuiyou
// Feedback: 	mailto:750539605@qq.com
//=====================================================
#endregion

using UnityEngine;
namespace Neatly
{
    public class NeatlyBehaviour : MonoBehaviour
    {
        public bool IsEnable { get; private set; }
        public bool IsDestroy { get; private set; }

        protected virtual void OnEnable()
        {
            IsEnable = true;
        }

        protected virtual void OnDisable()
        {
            IsEnable = false;
        }

        protected virtual void OnDestroy()
        {
            IsDestroy = true;
        }
    }

}
