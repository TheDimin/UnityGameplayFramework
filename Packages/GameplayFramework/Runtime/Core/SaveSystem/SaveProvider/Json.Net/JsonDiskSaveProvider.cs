using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace TowerJump.Save.provider
{
    public class JsonDiskSaveProvider : SaveProviderBase
    {
        protected JObject openJObject;
        protected JObject openComponent;
        protected GuidComponent currentGuidComponent;

        protected override void BeginObjectSave(GameObject gameObject, GuidComponent guidComponent)
        {
            openJObject = new JObject();
            currentGuidComponent = guidComponent;

        }

        protected override void BeginComponentSave(string name)
        {
            if (openComponent != null)
                throw new Exception("Cannot Begin new BeginComponentSave before FinishComponentSave");

            openComponent = new JObject();
            openJObject.Add(new JProperty(name, openComponent));
        }

        protected override void AddMember(string memberName, object memberValue)
        {
            openComponent.Add(new JProperty(memberName, JToken.FromObject(memberValue)));
        }

        protected override void FinishComponentSave()
        {
            if (openComponent == null)
                throw new Exception("Cannot FinishComponentSave when there is no active Component");


            openComponent = null;
        }

        protected override void FinishObjectSave()
        {
            if (currentGuidComponent == null)
                throw new NullReferenceException("Guid is null");

            Debug.Log(GetSavePath(currentGuidComponent.GetGuid()));

            using (var file = File.CreateText(GetSavePath(currentGuidComponent.GetGuid())))
            using (var writer = new JsonTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                openJObject.WriteTo(writer);
            }


        }

        public override bool DoesItemExist(Guid guid)
        {
            return File.Exists(GetSavePath(guid));
        }

        public override void CreateSaveSlot(bool replace = false)
        {
            var directory = GetDirectory();
            if (Directory.Exists(directory))
                if (replace)
                    Directory.Delete(directory);

            Directory.CreateDirectory(directory);
        }

        public static string GetDirectory()
        {
            var filepath = GetSavePath(new Guid());

            return filepath.Substring(0, filepath.LastIndexOf('/'));
        }

        public static string GetSavePath(Guid guid)
        {
            const int saveSlot = 1; //TODO get saveslot from somewhere else ?
            return $"{Application.persistentDataPath}/SaveGame/{saveSlot}/{guid}.json";
        }
    }
}