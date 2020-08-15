using GameplayFramework.Core.Internal;
using GameplayFramework.Core.Internal.State;

namespace GameplayFramework.Core
{
    public static class GameStateFactory
    {
        public static GameState GetGameState<GameState>() where GameState :GameStateBase
        {
            return Game.FindState<GameState>();
        }
    }
}