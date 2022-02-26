﻿using UnityEngine;

namespace Assets.Scripts.InterfaceTesting.TestableElements.Base
{
    public abstract class UIElement : MonoBehaviour, ITestable
    {
        public void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
        }

        public abstract void Simulate();
    }
}