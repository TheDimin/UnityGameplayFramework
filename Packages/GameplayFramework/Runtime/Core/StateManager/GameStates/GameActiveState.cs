namespace GameplayFramework.Core.Internal.State
{
    using GameplayFramework.State;
    using UnityEngine.SceneManagement;

    public class GameActiveState : GameStateBase
    {

        GamemodeBase GameModeInstance;

        public GameActiveState(GameplayFramework.Core.Internal.Game game) : base(game)
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
            //GameModeInstance = Game.gameDefaults.levels[SceneManager.GetActiveScene()].gamemode;
        }

        public override void OnExitState()
        {

        }
    }
}