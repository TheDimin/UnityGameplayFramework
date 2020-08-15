using UnityEngine;
using GameplayFramework.State;

namespace GameplayFramework.UI
{
    public abstract class MenuComponentBase : MonoBehaviour
    {
        protected MenuBase owner { get; private set; }

        public void Register(MenuBase Menu)
        {
            if (owner != null)
            {
                owner = Menu;
            }
            else
                Debug.LogError(string.Format("'{0}' Attempted to register '{1}' as owner. Already has a owner"));
        }

        public virtual void OnStateChange(StateBase StateBase) { }
    }

}