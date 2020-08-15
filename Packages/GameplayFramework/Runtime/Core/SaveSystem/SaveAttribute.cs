using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TowerJump.Save
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SaveAttribute : Attribute
    {
        public string SaveName { get; set; }
        //
        public SaveAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SaveClassAttribute : Attribute
    {
        /// <summary>
        /// When true the transform of the object will be saved (Transform will not be saved if the object is static)
        /// Otherwise ignore transform
        /// </summary>
        public bool SaveTransform = true;

    }
}
