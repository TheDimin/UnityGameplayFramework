using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameplayFramework.Core
{

    /// <summary>
    /// Object Representing the game
    /// Custom Window will allow you to edit these settings
    /// </summary>
    public class GameDefaults : ScriptableObject
    {
        public struct LevelSettings
        {
            [SerializeField]
            public GamemodeBase gamemode;

            [SerializeField]
            public GameObject PlayerPawn;  
        }

        /// <summary>
        /// Config for a level
        /// key is the level name 
        /// value are the settings for this level
        /// </summary>
        [SerializeField]
        public Dictionary<string,LevelSettings> levels;

        [SerializeField]
        //TODO editor window
        public string MainMenuScene;
    }
}
