using System;
using UnityEngine;

namespace UIFramework.Utils
{
    /// <summary>
    /// Singleton base for game object component.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _shuttingDown = false;

        public static T Instance
        {
            get
            {
                if (_shuttingDown)
                {
                    // Debug.LogWarning("Instance '" + typeof(T).Name + "' already destroyed. Returning null.");
                    return null;
                }
                
                return _instance ? _instance : 
                    throw new InvalidOperationException($"Pre-instantiation missing. An instance of {typeof(T).Name} is required to exist in the scene.");
            }
        }

        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }

        protected virtual void OnDestroy()
        {
            if (!_shuttingDown && _instance != null)
            {
                Debug.LogWarningFormat("Destroy MonoSingleton {0}", _instance.name);
            }

            if (_instance == this)
            {
                _instance = null;
            }
        }
        
        protected virtual void OnApplicationQuit()
        {
            _shuttingDown = true;
        }
    }
}