using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// Base interface for all the window params
    /// </summary>
    public interface IWindowParams { }

    /// <summary>
    /// All UI window behaviour should extend from this
    /// </summary>
    public abstract class BaseWindowBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Called when window/popup is opened (window gameObject become active)
        /// </summary>
        public virtual void OnOpenWindow(IWindowParams windowParams) { }

        /// <summary>
        /// Called when window/popup is closed (window gameObject become inactive)
        /// </summary>
        public virtual void OnCloseWindow() { }

        /// <summary> 
        /// Called when window canvas is shown (window become top when previous top window closes) 
        /// Works for windows only.
        /// </summary>
        public virtual void OnShowWindow() { }

        /// <summary>
        /// Called when window canvas is hidden (another window is opened on top)
        /// Works for windows only.
        /// </summary>
        public virtual void OnHideWindow() { }
    }

}