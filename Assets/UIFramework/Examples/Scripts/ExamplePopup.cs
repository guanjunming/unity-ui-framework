using TMPro;
using UnityEngine;

namespace UIFramework.Examples
{
    public class ExamplePopupParams : IWindowParams
    {
        public string title;
        public string content;

        public ExamplePopupParams(string title, string content)
        {
            this.title = title;
            this.content = content;
        }
    }

    public class ExamplePopup : BaseWindowBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;

        public override void OnOpenWindow(IWindowParams windowParams)
        {
            ExamplePopupParams arg = windowParams as ExamplePopupParams;
            titleText.text = arg.title;
            contentText.text = arg.content;
        }

        public void UI_OnClickOK()
        {
            UIManager.CloseWindow(WindowType.ExamplePopup);
        }
    }
}
