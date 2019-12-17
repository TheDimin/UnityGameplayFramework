

namespace GameplayFramework.Core.Internal.State
{
    using GameplayFramework.State;

    public abstract class GameStateBase : StateBase
    {

        public Game game { get; private set; }

        internal GameStateBase(Game Game)
        {
            this.game = Game;
        }

        public override bool CanEnter(StateBase CurrentStateBase)
        {
            return true;
        }
    }
}