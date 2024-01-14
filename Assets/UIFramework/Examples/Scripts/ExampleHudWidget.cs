namespace UIFramework.Examples
{
    public class ExampleHudWidget : BaseHUDWidgetBehaviour
    {
        public void OpenExampleWindow()
        {
            UIManager.OpenWindow(WindowType.ExampleWindow, new ExampleWindowParams("Example Title"));
        }

        public void OpenExamplePopup()
        {
            UIManager.OpenWindow(WindowType.ExamplePopup, new ExamplePopupParams("Confirm?", "Click button below to close popup"));
        }
    }
}