// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitySingletonBase.cs" company="Trollpants Game Studios AS">
//  Copyright (c) 2014 All Rights Reserved
// </copyright>
// <summary>
//  todo [Write a short summary of the script here]
// </summary>
// --------------------------------------------------------------------------------------------------------------------

 using UnityEngine;

    public abstract class UnitySingletonBase : MonoBehaviour
    {
        protected abstract void OnDestroy();

        protected abstract void OnApplicationQuit();
    }
