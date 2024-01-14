using TMPro;
using UnityEngine;

namespace UIFramework.Examples
{
    public class ExampleWindowParams : IWindowParams
    {
        public string title;

        public ExampleWindowParams(string title)
        {
            this.title = title;
        }
    }

    public class ExampleWindow : BaseWindowBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;

        public override void OnOpenWindow(IWindowParams windowParams)
        {
            ExampleWindowParams arg = windowParams as ExampleWindowParams;
            titleText.text = arg.title;
        }

        public void UI_OnClickButton()
        {
            UIManager.ShowSystemMessage("Example system message");
        }
    }
}

