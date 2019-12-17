using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameplayFramework.State
{
    public interface ISubState
    {
        StateBase GetActiveSubState();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="StateManager"></typeparam>
        /// <typeparam name="State"></typeparam>
        /// <returns></returns>
        StateManager GetSubStateManager<StateManager>() where StateManager : class;
    }

    public abstract class StateBase
    {
        public abstract void OnEnterState();
        public abstract void OnExitState();

        public abstract bool CanEnter(StateBase CurrentStateBase);
        public abstract bool CanExit();

        public virtual void Update() { }
        public virtual void FixedUpdate() {}
    }
}
