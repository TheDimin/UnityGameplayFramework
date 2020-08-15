using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UGF.Debug
{
    public class DrawForwardArrow
    {
        static void Draw(GameObject gameobject, float lenght = 5, float size = 0.1f)
        {
            Transform transform = gameobject.transform;
            Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(transform.forward * lenght, transform.up), size, EventType.Repaint);
        }
    }
}