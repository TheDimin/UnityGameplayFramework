using GameplayFramework.State;
using UnityEngine;
using UnityEngine.UI;
using GameplayFramework.UI;

[RequireComponent(typeof(Text))]
public class GameNameTextComponent : MenuComponentBase
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = Application.productName;
    }
}
