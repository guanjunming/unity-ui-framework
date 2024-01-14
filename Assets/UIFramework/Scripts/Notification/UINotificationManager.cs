using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    public class UINotificationManager : MonoBehaviour
    {
        private Dictionary<NotificationType, NotificationComponent> notificationMap = new Dictionary<NotificationType, NotificationComponent>();
        private SystemMessageController sysMsgController;

        private void Awake()
        {
            UIManager.RegisterNotificationManager(this);

            var components = gameObject.GetComponentsInChildren<NotificationComponent>(true);
            for (int i = 0; i < components.Length; i++)
            {
                components[i].RegisterNotification();
            }
        }

        private void OnDestroy()
        {
            notificationMap.Clear();
            sysMsgController = null;
        }

        public void RegisterNotificationComponent(NotificationComponent component)
        {
            NotificationType notificationType = component.notificationType;

            if (notificationMap.ContainsKey(notificationType))
            {
                Debug.LogErrorFormat("Duplicate notificationType {0} registered with UINotificationManager", notificationType.ToString());
            }
            else
            {
                notificationMap.Add(notificationType, component);
                if (notificationType == NotificationType.SystemMessage)
                    sysMsgController = component.GetComponent<SystemMessageController>();
            }
        }

        public void ShowSystemMessage(string message)
        {
            if (sysMsgController != null)
                sysMsgController.AddSystemMessage(message);
        }
    }
}