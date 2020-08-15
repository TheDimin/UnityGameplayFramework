using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.UI
{
    [DisallowMultipleComponent()]
    public abstract class MenuBase : MonoBehaviour
    {
        protected MenuComponentBase[] MenuComponents;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            GetMenuReferences();
        }

        private void GetMenuReferences()
        {
            MenuComponents = transform.GetComponentsInChildren<MenuComponentBase>(true);

            foreach (var menuComponent in MenuComponents)
            {
                menuComponent.Register(this);
            }
        }
    }
}