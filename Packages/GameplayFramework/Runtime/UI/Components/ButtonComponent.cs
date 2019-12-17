using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayFramework.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonComponent :  MenuComponentBase
    {
        [SerializeField]
        string eventName = "";

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}