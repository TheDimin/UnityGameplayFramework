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

        public virtual void Update()
        {
            if (currentState != null)
            {
                if (currentState.CanExit())
                {

                    Debug.Log("exiting: " + currentState.ToString());

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

        protected virtual void SetCurrentState(GameStateType newActivestate)
        {
            if (newActivestate == null) return;

            if(currentState != null)
                currentState.OnExitState();

            currentState = newActivestate;
            newActivestate.OnEnterState();

            OnStateChangeEvent?.Invoke(currentState);

            Debug.Log($"[{this.ToString()}]: Changed state to '{currentState.ToString()}'");
        }

        public GameStateType GetState()
        {
            return currentState;
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