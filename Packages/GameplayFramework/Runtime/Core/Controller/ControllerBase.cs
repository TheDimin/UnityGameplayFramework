using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Core
{
    public abstract class ControllerBase
    {
        private PawnBase possedPawn;

        public ControllerBase()
        {
            
        }

        public abstract void Awake();

        public void Posses(PawnBase targetPawn)
        {
            if(targetPawn == null)
            {
                Debug.LogError($"{this.ToString()} attempted to posses a null object");
                return;
            }
            if (possedPawn != null)
                possedPawn.UnPossessInternal(this);

            if (targetPawn.PossessInternal(this))
            {
                possedPawn = targetPawn;
            }
        }
    }
}