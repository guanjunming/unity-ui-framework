using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    public static class UIManager
    {
        public static UIHierarchy UIHierarchy;
        public static UIHud UIHud;
        public static UINotificationManager NotificationManager;

        private static Dictionary<HUDWidgetType, HUDWidget> widgetsMap = new Dictionary<HUDWidgetType, HUDWidget>();
        private static Dictionary<WindowType, UIWindow> windowsMap = new Dictionary<WindowType, UIWindow>();
        private static WindowStack<WindowType> windowStack = new WindowStack<WindowType>();
        private static WindowStack<WindowType> popupStack = new WindowStack<WindowType>();

        private static GameObject windowBlocker;
        public static GameObject WindowBlocker
        {
            get { return windowBlocker; }
            set { windowBlocker = value; }
        }

        /// <summary>
        /// Event when a window or popup is opened
        /// </summary>
        public static event Action<WindowType> WindowOpenedEvent;

        /// <summary>
        /// Event when a window or popup is closed
        /// </summary>
        public static event Action<WindowType> WindowClosedEvent;

        public static void RegisterUIHierarchy(UIHierarchy uihierarchy)
        {
            UIHierarchy = uihierarchy;
        }

        public static void RegisterHUD(UIHud hud)
        {
            UIHud = hud;
        }

        public static void RegisterWidget(HUDWidget widget)
        {
            HUDWidgetType widgetType = widget.widgetType;

            if (widgetsMap.ContainsKey(widgetType))
            {
                var widgetGO = widgetsMap[widgetType].gameObject;
                Debug.LogErrorFormat("Duplicate widgetType [{0}] registered with UI Manager. Check [{1}] and [{2}]", widgetType, widgetGO.name, widget.gameObject.name);
            }
            else
            {
                widgetsMap.Add(widgetType, widget);
            }
        }

        public static void RegisterWindow(UIWindow uiWindow)
        {
            WindowType windowType = uiWindow.windowType;
            if (windowsMap.ContainsKey(windowType))
            {
                var window = windowsMap[windowType].gameObject;
                Debug.LogErrorFormat("Duplicate windowType [{0}] registered with UI Manager. Check [{1}] and [{2}]", windowType, window.name, uiWindow.gameObject.name);
            }
            else
            {
                windowsMap.Add(windowType, uiWindow);
            }
        }

        public static void RegisterNotificationManager(UINotificationManager notificationMgr)
        {
            NotificationManager = notificationMgr;
        }

        /// <summary>
        /// To be called when destroying UIHierarchy
        /// </summary>
        public static void ClearUIHierarchy()
        {
            UIHierarchy = null;
            UIHud = null;
            NotificationManager = null;
            windowsMap.Clear();
            widgetsMap.Clear();
            windowStack.Clear();
            popupStack.Clear();
        }

        #region NotificationManager

        public static void ShowSystemMessage(string message)
        {
            if (NotificationManager != null)
                NotificationManager.ShowSystemMessage(message);
        }

        #endregion


        public static UIWindow GetWindow(WindowType windowType)
        {
            if (windowsMap.ContainsKey(windowType))
            {
                return windowsMap[windowType];
            }
            return null;
        }

        public static void OpenWindow(WindowType windowType, IWindowParams windowParams = null)
        {
            if (windowsMap.ContainsKey(windowType) && !windowsMap[windowType].gameObject.activeSelf)
            {
                if (windowType < WindowType.WindowEnd)
                {
                    HideTopWindow();
                    UIHud.HideHUD();
                }

                windowsMap[windowType].OpenWindow(windowParams);
                WindowOpenedEvent?.Invoke(windowType);
            }
        }

        public static void CloseWindow(WindowType windowType)
        {
            if (windowsMap.ContainsKey(windowType) && windowsMap[windowType].gameObject.activeSelf)
            {
                windowsMap[windowType].ClickClose();
            }
        }

        public static void CloseTopWindow()
        {
            if (windowStack.Count > 0)
            {
                var windowType = windowStack.Peek();
                CloseWindow(windowType);
            }
        }

        public static void CloseTopPopup()
        {
            if (popupStack.Count > 0)
            {
                var windowType = popupStack.Peek();
                CloseWindow(windowType);
            }
        }

        public static void CloseAllWindows()
        {
            while (windowStack.Count > 0)
            {
                var windowType = windowStack.Peek();
                CloseWindow(windowType);
            }
        }

        public static void CloseAllPopups()
        {
            while (popupStack.Count > 0)
            {
                var windowType = popupStack.Peek();
                CloseWindow(windowType);
            }
        }

        private static void HideTopWindow()
        {
            if (windowStack.Count > 0)
            {
                var topWindow = windowStack.Peek();
                windowsMap[topWindow].ShowCanvas(false);
            }
        }

        #region OpenedWindows

        /// <summary>
        /// Push window into stack
        /// </summary>
        public static void OnWindowOpened(WindowType windowType)
        {
            if (!windowStack.Contains(windowType))
            {
                windowStack.Push(windowType);
            }
        }

        public static void OnPopupOpened(WindowType windowType)
        {
            if (!popupStack.Contains(windowType))
            {
                popupStack.Push(windowType);
            }
        }

        /// <summary>
        /// Remove window from stack and show previous window
        /// </summary>
        public static void OnWindowClosing(WindowType windowType)
        {
            if (windowStack.Count > 0)
            {
                WindowBlocker.SetActive(true);

                //show next active window
                if (windowStack.Peek() == windowType)
                {
                    windowStack.Pop();
                    if (windowStack.Count > 0)
                    {
                        windowsMap[windowStack.Peek()].ShowCanvas(true);
                    }
                }
                else
                {
                    //handle closing of non topmost window
                    if (windowStack.Contains(windowType))
                    {
                        windowsMap[windowType].ShowCanvas(true);   // turn back on canvas
                        windowStack.Remove(windowType);
                    }
                }
            }
        }

        public static void OnPopupClosing(WindowType windowType)
        {
            if (popupStack.Count > 0)
            {
                WindowBlocker.SetActive(true);

                //show next active window
                if (popupStack.Peek() == windowType)
                {
                    popupStack.Pop();
                }
                else
                {
                    //handle closing of non topmost popup
                    if (popupStack.Contains(windowType))
                    {
                        popupStack.Remove(windowType);
                    }
                }
            }
        }

        /// <summary>
        /// Handle things to do when window closed is last window
        /// </summary>
        private static void OnWindowClosed(WindowType windowType)
        {
            if (windowStack.Count == 0)
            {
                UIHud.ShowHUD();
            }
        }

        public static void OnCloseWindow(UIWindow uiWindow)
        {
            WindowType windowType = uiWindow.windowType;
            if (windowsMap.ContainsKey(windowType))
            {
                WindowBlocker.SetActive(false);
                windowsMap[windowType].InternalCloseWindow();  // Set window inactive
                WindowClosedEvent?.Invoke(windowType);
                OnWindowClosed(windowType);  // Show HUD
            }
        }

        public static void OnClosePopup(UIWindow uiWindow)
        {
            WindowType windowType = uiWindow.windowType;
            if (windowsMap.ContainsKey(windowType))
            {
                WindowBlocker.SetActive(false);
                windowsMap[windowType].InternalCloseWindow();  // Set window inactive
                WindowClosedEvent?.Invoke(windowType);
            }
        }

        public static bool IsWindowOpen(WindowType windowType)
        {
            if (windowsMap.ContainsKey(windowType))
            {
                return windowsMap[windowType].gameObject.activeSelf;
            }
            return false;
        }

        #endregion OpenedWindows

        #region Window Behaviour

        public static BaseWindowBehaviour GetWindowBehaviour(WindowType windowType)
        {
            if (windowsMap.ContainsKey(windowType))
                return windowsMap[windowType].GetWindowBehaviour();
            else
                return null;
        }

        public static BaseWindowBehaviour GetTopWindowBehaviour()
        {
            if (windowStack.Count > 0)
                return windowsMap[windowStack.Peek()].GetWindowBehaviour();
            else
                return null;
        }

        #endregion Window Behaviour

        #region Widget

        public static HUDWidget GetHUDWidget(HUDWidgetType widgetType)
        {
            if (widgetsMap.ContainsKey(widgetType))
            {
                return widgetsMap[widgetType];
            }
            return null;
        }

        public static void SetHUDWidgetActive(HUDWidgetType widgetType, bool active)
        {
            if (widgetsMap.ContainsKey(widgetType))
            {
                widgetsMap[widgetType].SetActive(active);
            }
        }

        public static bool IsHUDWidgetActive(HUDWidgetType widgetType)
        {
            if (widgetsMap.ContainsKey(widgetType))
            {
                return widgetsMap[widgetType].IsActive();
            }
            return false;
        }

        #endregion Widget
    }

    internal class WindowStack<T> : System.Collections.IEnumerable
    {
        private List<T> items = new List<T>();

        public int Count { get { return items.Count; } }

        public void Push(T item)
        {
            items.Add(item);
        }

        public T Pop()
        {
            if (items.Count > 0)
            {
                T temp = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return temp;
            }
            else
                return default(T);
        }

        public void Remove(int itemAtPosition)
        {
            items.RemoveAt(itemAtPosition);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        public T Peek()
        {
            return items[items.Count - 1];
        }

        public T Get(int index)
        {
            return items[index];
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T obj)
        {
            return items.Contains(obj);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T this[int i]
        {
            get
            {
                return items[i];
            }
            set
            {
                items[i] = value;
            }
        }
    }
}