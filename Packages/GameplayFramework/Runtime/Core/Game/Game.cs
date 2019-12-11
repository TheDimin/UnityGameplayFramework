using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Core.Internal
{
    //Entry point of gameplay framework

    public class Game
    {
        private static Game game;

        public GamemodeBase gameMode { get; private set; }
        public GameDefaults gameDefaults { get; private set; }

        private Game()
        {
            game = this;

            gameDefaults = Resources.Load("GameSettings") as GameDefaults;

            gameMode = new ExampleGameMode();
            gameMode.Start();

            gameMode.RegisterPlayerController(new ExamplePlayerController());
        }

        /// <summary>
        /// Start point of the Gameplay framework create the game instance.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            Debug.Log("AssembliesLoaded");
            game = new Game();

        }



        //Bad coding practice should be reworked at some point
        public static GamemodeBase GetGamemode()
        {
            return game.gameMode;
        }
        public static GameDefaults GetGameDefaults()
        {
            return game.gameDefaults;
        }
    }
}
