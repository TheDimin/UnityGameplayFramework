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
        [SerializeField]
        public GamemodeBase gamemode;

        public GameObject PlayerPawn;

        //TODO editor window
        public string MainMenuScene;
    }
}
