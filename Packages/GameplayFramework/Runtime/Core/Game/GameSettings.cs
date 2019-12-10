using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace GameplayFramework.Core
{
    public class GameSettings : ScriptableObject , ISerializationCallbackReceiver
    {
        [SerializeField]
        public GamemodeBase gamemode;

        private string GameModeClass = "";

        [SerializeField]
        public string a = "lul";

        public void OnAfterDeserialize()
        {
            Debug.Log($"gamemode full name :{GameModeClass}");
        }

        public void OnBeforeSerialize()
        {
            GameModeClass = gamemode.GetType().AssemblyQualifiedName;
        }
    }
}
