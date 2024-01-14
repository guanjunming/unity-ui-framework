namespace UIFramework
{
    public class UIPopup : UIWindow
    {
        public override void OnCloseClicked()
        {
            if (windowType != WindowType.None)
            {
                UIManager.OnClosePopup(this);
            }
        }

        public override void OpenWindow(IWindowParams windowParams)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);

                UIManager.OnPopupOpened(windowType);

                if (windowBehaviour != null)
                {
                    windowBehaviour.OnOpenWindow(windowParams);
                }
            }
        }

        public override void OnClosing()
        {
            UIManager.OnPopupClosing(windowType);

            if (!hasCloseAnimation)
            {
                OnCloseClicked();
            }
            else if (windowAnimator != null)
            {
                windowAnimator.Play("UI_Close");
            }

            if (windowBehaviour != null)
            {
                windowBehaviour.OnCloseWindow();
            }
        }
    }
}