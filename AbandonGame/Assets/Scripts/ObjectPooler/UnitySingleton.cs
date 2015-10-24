// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitySingleton.cs" company="Trollpants Game Studios AS">
//  Copyright (c) 2014 All Rights Reserved
// </copyright>
// <summary>
//  todo [Write a short summary of the script here]
// </summary>
// --------------------------------------------------------------------------------------------------------------------


#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    public class UnitySingleton<T> : UnitySingletonBase where T : Component
    {
        private static T s_instance;

        public static bool SingletonDestroyed
        {
            get;
            private set;
        }

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (EditorApplication.isCompiling)
                {
                    Debug.Log("IS COMPILING");
                    return null;
                }
#endif
                if (!ReferenceEquals(s_instance, null))
                {
                    return s_instance;
                }

                var singletons = FindObjectsOfType(typeof(T));

                if (!ReferenceEquals(singletons, null) && singletons.Length > 0)
                {
                    s_instance = singletons[0] as T;

                    if (singletons.Length > 1)
                    {
                        Debug.LogWarning(string.Format("Multiple instances of singleton type {0} found.", typeof(T)));
                        for (var i = 1; i < singletons.Length; i++)
                        {
                            Destroy(singletons[i]);
                        }
                    }
                }

                if (!ReferenceEquals(s_instance, null))
                {
                    return s_instance;
                }

                // In case the singleton is referenced somewhere in a scene that doesnt have the singleton component on an object
                // we create one and hide it.
#if UNITY_EDITOR || DEBUG
                Debug.Log("No Singleton Found, Populating. " + typeof(T));
#endif
                var obj = new GameObject
                {
                    name = string.Format("NewTransient{0}Singleton", typeof(T)),
                    hideFlags = HideFlags.HideAndDontSave
                };

                s_instance = obj.AddComponent(typeof(T)) as T; // Calls ctor of T
                return s_instance;
            }
        }

        protected virtual void OnOnDestroy()
        {
        }

        // [Obsolete("OnDestroy() is reserved for singleton, please use OnOnDestroy() instead.", false)]
        protected sealed override void OnDestroy()
        {
            SingletonDestroyed = true;
            OnOnDestroy();
            s_instance = null;
        }

        protected virtual void OnOnApplicationQuit()
        {
        }

        // [Obsolete( "OnApplicationQuit() is reserved for singleton, please use OnOnApplicationQuit() instead.", false )]
        protected sealed override void OnApplicationQuit()
        {
            OnOnApplicationQuit();
            DestroyImmediate(gameObject, false);
            s_instance = null;
        }
    }

