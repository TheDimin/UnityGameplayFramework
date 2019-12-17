using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayFramework.Core.Internal.State;
using GameplayFramework.State;

namespace GameplayFramework.Core.Internal
{
    //Entry point of gameplay framework

    public class Game
    {
        private static Game game;

        public StateManagerBase<GameStateBase> gameStateManager { get; private set; }
        public GameDefaults defaults { get; private set; } 

        private Game()
        {
            game = this;

            defaults = Resources.Load("GameSettings") as GameDefaults;

            gameStateManager = new StateManagerBase<GameStateBase>();
            RegisterGameStates();
        }

        private void RegisterGameStates()
        {
            gameStateManager.RegisterState(new MainMenuState(this));
            gameStateManager.RegisterState(new GameActiveState(this));
            //TODO
            // RegisterState(new GamePausedState());
            gameStateManager.RegisterState(new EndGameState(this));
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

        /// <summary>
        /// Find the current gameState
        /// </summary>
        /// <returns></returns>
        public static GameStateBase FindState()
        {
            return game.gameStateManager.GetState();
        }

        /// <summary>
        /// find gamestate of type
        /// </summary>
        /// <typeparam name="GameState"></typeparam>
        /// <returns></returns>
        public static GameState FindState<GameState>() where GameState : GameStateBase
        {
            return game.gameStateManager.FindState<GameState>();
        }
    }
}
