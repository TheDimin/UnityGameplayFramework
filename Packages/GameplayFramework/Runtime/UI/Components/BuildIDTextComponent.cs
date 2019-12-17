using GameplayFramework.State;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class BuildIDTextComponent : MenuComponentBase
    {
        void Start()
        {
            GetComponent<Text>().text = string.Format("{0}: {1}", Application.unityVersion, Application.isEditor ? "editor" : Application.buildGUID );
            
        }

        private void Update()
        {
            print(Application.isFocused);
        }
    }
}
