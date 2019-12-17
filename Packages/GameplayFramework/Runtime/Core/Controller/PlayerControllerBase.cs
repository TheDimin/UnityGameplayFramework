using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework.Core
{
    public abstract class PlayerControllerBase : ControllerBase 
    {
        public string Id { get; protected set; }
        protected PlayerControllerBase(string id)
        {
            Id = id;
        }
    }
}