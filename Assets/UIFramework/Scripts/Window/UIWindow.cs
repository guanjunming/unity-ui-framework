using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    public class UIWindow : MonoBehaviour
    {
        public WindowType windowType = WindowType.None;
        public Button[] closeButton;
        public Canvas[] windowCanvas;
        public GameObject[] objectsToHide;
        public Animator windowAnimator;
        public bool hasCloseAnimation = false;

        protected BaseWindowBehaviour windowBehaviour;

        public BaseWindowBehaviour GetWindowBehaviour()
        {
            return windowBehaviour;
        }

        /// <summary>
        /// added as an event handler to GUIAnimEvent OnFinished event
        /// </summary>
        public virtual void OnCloseClicked()
        {
            if (windowType != WindowType.None)
            {
                UIManager.OnCloseWindow(this);
            }
        }

        #region UIManager
        public virtual void RegisterWindow()
        {
            if (windowType != WindowType.None)
            {
                for (int i = 0; i < closeButton.Length; i++)
                {
                    if (closeButton[i] != null)
                        closeButton[i].onClick.AddListener(OnClosing);
                }

                windowBehaviour = gameObject.GetComponent<BaseWindowBehaviour>();
                UIManager.RegisterWindow(this);
            }
        }

        public virtual void OpenWindow(IWindowParams windowParams)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);  // Show window

                var topWindow = UIManager.GetTopWindowBehaviour();  // any previous window
                if (topWindow != null)
                    topWindow.OnHideWindow();

                UIManager.OnWindowOpened(windowType);  // Push into window queue

                if (windowBehaviour != null)  // Window to be opened
                {
                    windowBehaviour.OnOpenWindow(windowParams);
                }
            }
        }

        public void InternalCloseWindow()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Call this to close window by code
        /// </summary>
        public void ClickClose()
        {
            if (closeButton.Length > 0 && closeButton[0] != null)
                closeButton[0].onClick.Invoke();
            else
                OnClosing();
        }

        public void ShowCanvas(bool show)
        {
            for (int i = 0; i < windowCanvas.Length; i++)
            {
                if (windowCanvas[i] != null)
                    windowCanvas[i].enabled = show;
            }

            for (int i = 0; i < objectsToHide.Length; i++)
            {
                if (objectsToHide[i] != null)
                    objectsToHide[i].SetActive(show);
            }
        }
        #endregion

        public virtual void OnClosing()  // called by close button
        {
            UIManager.OnWindowClosing(windowType);  // remove window from queue and show previous window

            if (!hasCloseAnimation)
            {
                OnCloseClicked(); // set window inactive, no need to wait for close animation
            }
            else if (windowAnimator != null)
            {
                windowAnimator.Play("UI_Close");
            }

            if (windowBehaviour != null)
            {
                windowBehaviour.OnCloseWindow();
            }

            var topWindow = UIManager.GetTopWindowBehaviour();
            if (topWindow != null)
                topWindow.OnShowWindow();
        }
    }
}