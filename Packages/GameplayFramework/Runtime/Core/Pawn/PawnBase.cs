using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Core
{
    public abstract class PawnBase : MonoBehaviour
    {

        protected Camera camera { get; private set; }
        private ControllerBase controller;

        protected virtual void OnPossessed(ControllerBase controller) { }

        protected virtual void OnUnPossessed(ControllerBase controller) { }

        /// <summary>
        /// DO NOT CALL
        /// Called by the controller that attempts to posses the pawn
        /// use OnPossed instead
        /// </summary>
        /// <param name="controller">Controller that possed the pawn</param>
        /// <returns>Returns true when possessed the pawn succesfully</returns>
        internal bool PossessInternal(ControllerBase controller)
        {
            if (IsPossed())
            {
                Debug.LogWarning($"{controller.ToString()} attempted to posses '{this.ToString()}' but is already Possessed");
                return false;
            }

            this.controller = controller;
            OnPossessed(controller);
            return true;
        }

        internal bool UnPossessInternal(ControllerBase controller)
        {
            controller = null;
            return true;
        }

        public bool IsPossed()
        {
            return controller != null;
        }
    }
}