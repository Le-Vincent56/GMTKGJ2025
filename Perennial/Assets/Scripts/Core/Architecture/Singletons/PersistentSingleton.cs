using Sirenix.OdinInspector;
using UnityEngine;

namespace Perennial.Core.Architecture.Singletons
{
    public class PersistentSingleton<T> : SerializedMonoBehaviour where T : Component
    {
        public bool autoUnparentOnAwake = true;

        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance) return instance;
                
                // Try to set the instance by finding an object of type T
                instance = FindAnyObjectByType<T>();
                    
                // Check if the instance is still null
                if (instance) return instance;
                
                // Create a new game object and add the component of type T
                GameObject go = new GameObject(typeof(T).Name + " (Auto-Generated)");
                instance = go.AddComponent<T>();

                return instance;
            }
        }

        protected virtual void Awake()
        {
            // Initialize the Persistent Singleton
            InitializeSingleton();
        }

        /// <summary>
        /// Initialize the Persistent Singleton
        /// </summary>
        protected virtual void InitializeSingleton()
        {
            // Exit case - the application is not playing
            if (!Application.isPlaying) return;

            // Check if unparenting on awake
            if(autoUnparentOnAwake)
                // Unparent the Transform
                transform.SetParent(null);

            // Check if there is an instance
            if(!instance)
            {
                // If not, set this as the instance
                instance = this as T;

                // Set to not destroy on loading
                DontDestroyOnLoad(gameObject);
            } 
            else
            {
                // Otherwise, check if the instance is this component
                if(instance != this)
                {
                    // If not, then destroy this game object
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// Try to get the Instance of the Persistent Singleton
        /// </summary>
        public static T TryGetInstance() => instance ? instance : null;
    }
}
