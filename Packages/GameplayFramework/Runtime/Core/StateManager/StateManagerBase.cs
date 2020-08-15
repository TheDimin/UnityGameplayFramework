using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameplayFramework.State
{
    public class StateManagerBase<GameStateType> where GameStateType : StateBase
    {
        public delegate void GameStateChangeHandler(GameStateType newState);

        public event GameStateChangeHandler OnStateChangeEvent;
  

        protected List<GameStateType> registerdStates = new List<GameStateType>();
        protected GameStateType currentState;

        /// <summary>
        /// Update State loop and current state
        /// </summary>
        public virtual void Update()
        {
            if (currentState != null)
            {
                if (currentState.CanExit())
                {
                    for (int i = 0; i < registerdStates.Count; i++)
                    {
                        if (registerdStates[i] != currentState)
                        {
                            if (registerdStates[i].CanEnter(currentState))
                            {
                                SetCurrentState(registerdStates[i]);
                                break;
                            }
                        }
                    }
                }

                currentState.Update();
            }else
            { 
                Debug.LogWarning(this.ToString() + " has no current state");
            }
        }

        /// <summary>
        /// Adds a new state to the statemanager
        /// </summary>
        /// <param name="stateBase"></param>
        public virtual void RegisterState(GameStateType stateBase)
        {
            if (!registerdStates.Contains(stateBase))
            {
                registerdStates.Add(stateBase);
            }

            if (currentState == null)
            {
                if(stateBase.CanEnter(null))
                    SetCurrentState(stateBase);
            }
        }

        /// <summary>
        /// Force the state to enter specific state
        /// </summary>
        /// <param name="newActivestate"></param>
        protected virtual void SetCurrentState(GameStateType newActivestate)
        {
            if (newActivestate == null) return;

            if(currentState != null)
                currentState.OnExitState();

            currentState = newActivestate;
            newActivestate.OnEnterState();

            OnStateChangeEvent?.Invoke(currentState);

            Debug.Log($"[{ToString()}]: Changed state to '{currentState}'");
        }

        /// <summary>
        /// Returns the currently Active state
        /// </summary>
        /// <returns></returns>
        public GameStateType GetState()
        {
            return currentState;
        }

        /// <summary>
        /// Function will be called when a new state is enterdt
        /// </summary>
        /// <param name="action"></param>
        public void BindOnStateChange(GameStateChangeHandler action)
        {
            OnStateChangeEvent += action;
        }
        
        public void RemoveOnStateChangeBind(GameStateChangeHandler action)
        {
            OnStateChangeEvent -= action;
        }

        /// <summary>
        /// Returns the first State of type
        /// </summary>
        /// <typeparam name="State"></typeparam>
        /// <param name="AllowSubClass">When there is no state of type found are we allowed to return subclasses of type</param>
        /// <returns></returns>
        public State FindState<State>(bool AllowSubClass = false) where State : GameStateType
        {
            foreach (GameStateType registerdState in registerdStates)
            {
                if(registerdState.GetType() == typeof(State))
                {
                    return registerdState as State;
                }
            }

            if (AllowSubClass)
            {
                foreach (GameStateType registerdState in registerdStates)
                {
                    if (registerdState.GetType().IsSubclassOf(typeof(State)))
                    {
                        return registerdState as State;
                    }
                }
            }
            return null;
        }
    }
}