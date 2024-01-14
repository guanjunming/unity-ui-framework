using UnityEngine;

namespace UIFramework
{
    public class NotificationComponent : MonoBehaviour
    {
        public NotificationType notificationType;

        public void RegisterNotification()
        {
            if (notificationType != NotificationType.None)
            {
                UIManager.NotificationManager.RegisterNotificationComponent(this);
            }
        }
    }
}
