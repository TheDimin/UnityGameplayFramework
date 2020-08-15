//
// Thanks to dbc from:
//https://stackoverflow.com/questions/28451990/newtonsoft-json-deserialize-dictionary-as-key-value-list-from-datacontractjsonse/28633769
//
// <summary>Deserializes dictionaries.</summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace TowerJump.Save.Internal
{
    public class DictionaryConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var existingDictionary = existingValue as IDictionary;
            if (existingDictionary == null) throw new Exception("Type not dictionary"); //can't happen

            var ojb = JObject.Load(reader);
            foreach (var element in ojb.Children())
            {
                var jpropElement = element as JProperty;

                if (existingDictionary.Contains(jpropElement.Name))
                {
                    
                    var serializerSettings = new JsonSerializerSettings{Converters = new List<JsonConverter>{new DictionaryConverter()}};
                    //

                    JsonConvert.PopulateObject(jpropElement.Value.ToString() ,existingDictionary[jpropElement.Name], serializerSettings);
                }

            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary).IsAssignableFrom(objectType) && objectType.GenericTypeArguments[0] == typeof(string);
        }


    }
}