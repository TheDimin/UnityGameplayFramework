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
        /// <summary>
        /// Called when this state Has been entered
        /// </summary>
        public abstract void OnEnterState();
        /// <summary>
        /// Called when this state has been exited
        /// </summary>
        public abstract void OnExitState();

        /// <summary>
        /// Check if this state can be entered
        /// </summary>
        /// <param name="currentStateBase"></param>
        /// <returns></returns>
        public abstract bool CanEnter(StateBase currentStateBase);
        /// <summary>
        /// Check if this state (Thats currently the active state) Can be exited
        /// </summary>
        /// <returns></returns>
        public abstract bool CanExit();
        /// <summary>
        /// Called every frame while this is the active state
        /// (Make sure to call this function from the monobehaviour that owns the statemanager)
        /// </summary>
        public virtual void Update() { }
    }
}
