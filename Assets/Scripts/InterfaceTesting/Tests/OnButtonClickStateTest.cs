﻿using UnityEngine;

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