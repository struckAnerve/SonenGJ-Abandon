// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectPooler.cs" company="Trollpants Game Studios AS">
//  Copyright (c) 2014 All Rights Reserved
// </copyright>
// <summary>
//  This script will spawn and recycle game objects so the game can reuse objects,
//  instead of always creating new ones and destroying them. This helps maintain good performance.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Trollpants.UtilityBelt
{
    using UnityEngine;

    public class ObjectPooler : UnitySingleton<ObjectPooler>
    {
        #region Fields & properties

        public GameObject[] objectPool;

        private Transform _objectPoolTransform;


        #endregion /Fields & properties

        #region Public methods

        /// <summary>
        /// Spawn an object from a prefab, the ObjectPooler will either instantiate a new object or reuse an old one
        /// </summary>
        /// <param name="prefabPath">The path where the prefab is stored, the prefab MUST be stored in the Resources folder
        ///  and the string must conform to this example: "Prefabs/Enemies/"</param>
        /// <param name="prefabName">The name of the prefab (filename without extension)</param>
        /// <param name="parent">The parent of the new GameObject</param>
        /// <param name="worldPosition">Where in world space the object will be spawned</param>
        /// <returns>The spawned <see cref="GameObject"/></returns>
        public GameObject Spawn(string prefabName, Transform parent, Vector3 worldPosition, Quaternion rotation)
        {
            var poolGameObject = _objectPoolTransform.FindChild(prefabName);
            GameObject g = null;

            if (poolGameObject != null)
            {
                g = poolGameObject.gameObject;
                g.transform.position = worldPosition;
                g.transform.rotation = rotation;
            }
            else
            {
                foreach (var prefab in objectPool)
                {
                    if (prefab.name == prefabName)
                    {
                        g = Instantiate(prefab, worldPosition, rotation) as GameObject;
                        break;
                    }
                }
            }

            if (g != null)
            {
                g.name = prefabName;
                var rigidBody = g.GetComponent<Rigidbody>();
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;

                g.transform.SetParent(parent);
                g.SetActive(true);
                return g;
            }

#if DEBUG
            Debug.LogError("Spawn failed!");
#endif
            return null;
        }

        #endregion /Public methods

        #region Unity methods

        private void Awake()
        {
            _objectPoolTransform = gameObject.transform;
            _objectPoolTransform.name = "ObjectPooler";
        }
        #endregion /Unity methods
    }
}
