using GameplayFramework.State;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayFramework.Core.Internal.State
{
    /// <summary>
    /// De eerste state waar de game inzit,
    /// Displayed de tutorial en wacht op de input om naar de gamestate te gaan.
    /// </summary>
    public class MainMenuState : GameStateBase
    {
        public MainMenuState(GameplayFramework.Core.Internal.Game game) : base(game)
        {
        }

        public override bool CanEnter(StateBase currentStateBase)
        {
            return true;
        }

        public override bool CanExit()
        {
            return false;
        }

        public override void OnEnterState()
        {
            string mainMenuScene = game.defaults.MainMenuScene;

            if (mainMenuScene != "")
            {
                if (SceneManager.GetSceneByName(mainMenuScene) == null)
                {
                    Debug.LogWarning("Scene by this name not loaded, now loading");
                    SceneManager.LoadSceneAsync(mainMenuScene);
                    //TODO show user Transition scene
                }
            }
        }

        public override void OnExitState()
        {
        }

        public override void Update()
        {
        }
    }
}