
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TowerJump.Save.Internal;
using UnityEngine;

namespace TowerJump.Save.provider
{
    public abstract class LoadProviderBase
    {
        public void Start(SaveObjectInfo objectInfo)
        {
            if (!DoesSaveExist(objectInfo.GuidComponent))
                Debug.LogWarning($"Failed to find save for object with id {objectInfo.GetGuid}");

            BeginObjectLoad(objectInfo.GetGuid);

            var componentName = "";

            while (LoadNextComponent(ref componentName))
            {
                var component = objectInfo.GetComponentFromName(componentName);

                if (!component) continue;

                var componentType = component.GetType();

                while (GetNextMember(out var memberName))
                {
                    var field = componentType.GetField(memberName,
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    if (field != null)
                    {
                        var currentValue = field.GetValue(component);
                        UpdateMemberValue(ref currentValue);
                        field.SetValue(component, currentValue);
                    }
                    else
                    {
                        var property = componentType.GetProperty(memberName,
                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                        if (property == null) continue;

                        var currentValue = property.GetValue(component);
                        UpdateMemberValue(ref currentValue);
                        property.SetValue(component, currentValue);

                    }

                }
                FinishComponentLoad();
            }
            FinishObjectLoad();

        }



        protected abstract bool DoesSaveExist(GuidComponent guidComponent);

        protected abstract void BeginObjectLoad(Guid objectGuid);
        protected abstract bool LoadNextComponent(ref string componentName);

        /// <summary>
        /// Called for every property/field that wants to be saved. Handle where this element should be
        /// </summary>
        /// <param name="memberName">Name of the value</param>
        /// <param name="memberValue">value of a property/field</param>
        protected abstract bool GetNextMember(out string memberName);

        protected abstract void UpdateMemberValue(ref object member);

        protected abstract void FinishComponentLoad();
        protected abstract void FinishObjectLoad();
    }
}
