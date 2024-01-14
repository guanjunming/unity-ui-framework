using UnityEngine;

namespace UIFramework
{
    public class HUDWidget : MonoBehaviour
    {
        public HUDWidgetType widgetType;
        public bool ActiveOnStartup = true;

        private BaseHUDWidgetBehaviour widgetBehaviour;

        public BaseHUDWidgetBehaviour GetWindowBehaviour()
        {
            return widgetBehaviour;
        }

        public void RegisterWidget()
        {
            if (widgetType != HUDWidgetType.None)
            {
                widgetBehaviour = gameObject.GetComponent<BaseHUDWidgetBehaviour>();
                gameObject.SetActive(ActiveOnStartup);
                UIManager.RegisterWidget(this);
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}