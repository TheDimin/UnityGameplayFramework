using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TowerJump.Save.Internal;
using UnityEngine;

namespace TowerJump.Save.provider
{
    public class JsonDiskLoadProvider : LoadProviderBase
    {
        protected JObject _openObject;
        protected JObject _openComponent;
        protected JToken _openMember;

        protected IEnumerator<KeyValuePair<string, JToken>> _objectEnumerator;
        protected IEnumerator<KeyValuePair<string, JToken>> _componentEnumerator;



        protected override bool DoesSaveExist(GuidComponent guidComponent)
        {
            return File.Exists(JsonDiskSaveProvider.GetSavePath(guidComponent.GetGuid()));
        }

        protected override void BeginObjectLoad(Guid objectGuid)
        {
            var serializerSettings = new JsonSerializerSettings{Converters = new List<JsonConverter>{new DictionaryConverter()}};
            using (StreamReader streamReader = File.OpenText(JsonDiskSaveProvider.GetSavePath(objectGuid)))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                _openObject = (JObject)JToken.ReadFrom(reader);
                _objectEnumerator = _openObject.GetEnumerator();
            }
        }

        protected override bool LoadNextComponent(ref string componentName)
        {
            if (!_objectEnumerator.MoveNext())
                return false;

            var currentComponent = _objectEnumerator.Current;
            componentName = currentComponent.Key;
            _componentEnumerator = (currentComponent.Value as JObject)?.GetEnumerator();

            return true;
        }

        protected override bool GetNextMember(out string memberName)
        {
            if (!_componentEnumerator.MoveNext())
            {
                memberName = "Null";
                return false;
            }

            var member = _componentEnumerator.Current;
            memberName = member.Key;
            _openMember = member.Value;
            return true;
        }

        protected override void UpdateMemberValue(ref object member)
        {
            if (_openMember == null)
            {
                throw new NullReferenceException("Can't get Membervalue. GetNextMember has to be called first!");
            }

            
            //serializerSettings
            
            new JsonSerializer().Populate(_openMember.CreateReader(),member);
            //member = null;
        }

        protected override void FinishComponentLoad()
        {
            _componentEnumerator.Dispose();
            _componentEnumerator = null;
        }

        protected override void FinishObjectLoad()
        {
            _objectEnumerator.Dispose();
            _objectEnumerator = null;
        }
    }

}