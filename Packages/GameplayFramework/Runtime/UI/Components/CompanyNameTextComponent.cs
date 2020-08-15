using UnityEngine;
using UnityEngine.UI;

namespace GameplayFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class CompanyNameTextComponent : MenuComponentBase
    {
        void Start()
        {
            GetComponent<Text>().text = Application.companyName;
        }
    }
}