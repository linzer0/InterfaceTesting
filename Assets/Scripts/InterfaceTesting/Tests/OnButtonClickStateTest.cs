<<<<<<< HEAD
﻿using UnityEngine;
=======
﻿using Assets.Scripts.InterfaceTesting.Tests;
using InterfaceTesting.TestableElements;
using UnityEngine;
>>>>>>> 92c50e979e2243e1fa6d05930c3a282692178337

namespace InterfaceTesting
{
    public class OnButtonClickStateTest : UIElementStateTest
    {
        [SerializeField] private TestableButton _testableButton;

        private bool _targetStartState;

        public override void RunTest()
        {
            _targetStartState = _targetElement.activeSelf;
            _testableButton.Simulate();

            if (_targetElement.activeSelf == _expectedState)
            {
                InvokeResult(false);
            }
            else
            {
                InvokeResult(true,
                    $"{_targetElement.name} state is {_targetElement.activeSelf} expected {_expectedState}");
            }

            _targetElement.SetActive(_targetStartState);
        }

        public override string GetDescription()
        {
            return $"Target object is {_targetElement.gameObject.name} and Target state is {GetExpectedState()}";
        }
    }
}