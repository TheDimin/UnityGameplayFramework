using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using TowerJump.Save.Internal;
using UnityEngine;

namespace TowerJump.Save.provider
{
    public abstract class SaveProviderBase
    {
        public SaveProviderBase()
        {
            //TODO
        }

        #region SaveProcces
        protected abstract void BeginObjectSave(GameObject gameObject, GuidComponent guidComponent);

        protected abstract void BeginComponentSave(string name);
        /// <summary>
        /// Called for every property/field that wants to be saved. Handle where this element should be
        /// </summary>
        /// <param name="memberName">Name of the value</param>
        /// <param name="memberValue">value of a property/field</param>
        protected abstract void AddMember(string memberName, object memberValue);

        protected abstract void FinishComponentSave();
        protected abstract void FinishObjectSave();
        #endregion

        /// <summary>
        /// Tell the save provider to save the object
        /// </summary>
        /// <param name="targetObject"></param>
        public virtual void Start(SaveObjectInfo targetObject)
        {
            CreateSaveSlot(false); // make sure we have a save slot we can write to

            BeginObjectSave(targetObject.GameObject, targetObject.GuidComponent);
            SaveObject_internal(targetObject);
            FinishObjectSave();

        }

        public abstract bool DoesItemExist(Guid guid);

        public abstract void CreateSaveSlot(bool replace = false);

        private void SaveObject_internal(SaveObjectInfo targetObjectInfo)
        {
            /*
            BeginComponentSave(nameof(GameObject)); // not realy a component but "its works"
            {
                var obj = targetObjectInfo.GameObject;

                GetNextMember("activeInHierarchy", obj.activeInHierarchy);
                GetNextMember("Tag",obj.tag);
            }
            FinishComponentSave();
            */

            foreach (var saveComponent in targetObjectInfo.GetComponents())
            {
                BeginComponentSave(saveComponent.GetType().Name);

                if (saveComponent.GetType() == typeof(Transform))
                {
                    var transform = saveComponent.transform;
                    AddMember("position", transform.localPosition);
                    AddMember("localScale", transform.localScale);
                    AddMember("eulerAngles", transform.localEulerAngles);
                }
                else
                {

                    foreach (var property in saveComponent.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        if (property.IsDefined(typeof(SaveAttribute)) && property.CanRead && property.CanWrite)
                        {
                            AddMember(property.Name, property.GetValue(saveComponent));
                            //Debug.Log($"property: {} value = {property.GetValue(saveComponent)}");
                        }
                    }

                    foreach (var field in saveComponent.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        if (field.IsDefined(typeof(SaveAttribute)) && !field.IsLiteral)
                        {
                            AddMember(field.Name, field.GetValue(saveComponent));
                            //Debug.Log($"field: {field.Name} value = {field.GetValue(saveComponent)}");
                        }
                    }
                }
                FinishComponentSave();
            }
        }
    }
}
