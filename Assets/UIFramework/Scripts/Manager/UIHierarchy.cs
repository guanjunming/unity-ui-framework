using UnityEngine;

namespace UIFramework
{
    public class UIHierarchy : MonoBehaviour
    {
        public GameObject windowBlocker;

       private void Awake()
        {
            UIManager.RegisterUIHierarchy(this);
            UIManager.WindowBlocker = windowBlocker;

            var windowComponents = gameObject.GetComponentsInChildren<UIWindow>(true);
            for (int i = 0; i < windowComponents.Length; ++i)
            {
                windowComponents[i].RegisterWindow();
            }
        }

        private void OnDestroy()
        {
            UIManager.ClearUIHierarchy();
        }
    }
}