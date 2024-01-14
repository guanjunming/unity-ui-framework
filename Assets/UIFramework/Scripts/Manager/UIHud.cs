using UnityEngine;

namespace UIFramework
{
    public class UIHud : MonoBehaviour
    {
        public Canvas canvas;

        private void Awake()
        {
            UIManager.RegisterHUD(this);

            var widgetComponents = gameObject.GetComponentsInChildren<HUDWidget>(true);
            for (int i = 0; i < widgetComponents.Length; i++)
            {
                widgetComponents[i].RegisterWidget();
            }
        }

        public void ShowHUD()
        {
            canvas.enabled = true;
        }

        public void HideHUD()
        {
            canvas.enabled = false;
        }

        public bool IsVisible()
        {
            return canvas.enabled;
        }
    }
}
