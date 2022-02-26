using InterfaceTesting.Tests;
using UnityEngine;

namespace Assets.Scripts.InterfaceTesting.Tests
{
    public class UIElementStateTest : BaseTest
    {
        [SerializeField] protected GameObject _targetElement;
        [SerializeField] protected bool _expectedState;

        public override void RunTest()
        {
            if (_targetElement.activeSelf == _expectedState)
            {
                InvokeResult(false);
            }
            else
            {
                InvokeResult(true,
                    $"{_targetElement.name} state is {_targetElement.activeSelf} expected {_expectedState}");
            }
        }
    }
}