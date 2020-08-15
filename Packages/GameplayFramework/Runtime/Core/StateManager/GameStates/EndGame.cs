using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// De state op het einde van de game,
/// wacht op een bepaalde input om de game te resetten.
/// </summary>

/* todo
 * 
 * Disable player movement
 * Show 'code' on second screen*
 * Disable camera's
 * Allow player to reset game to the begining.
 */

namespace GameplayFramework.Core.Internal.State
{
    using GameplayFramework.Core.Internal;
    using GameplayFramework.State;

    public class EndGameState : GameStateBase
    {

        public EndGameState(Game game) : base(game)
        {
            //TODO implement code
        }

        public override bool CanEnter(StateBase currentStateBase)
        {
            return false;
        }

        public override bool CanExit()
        {
            return Input.GetKey(KeyCode.F1);
        }

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void Update()
        {

        }

        private void RestartGame()
        {

        }
    }
}