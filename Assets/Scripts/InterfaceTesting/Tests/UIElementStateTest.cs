using UnityEngine;

namespace InterfaceTesting
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

        public string GetExpectedState()
        {
            return _expectedState ? "Active" : "Inactive";
        }

        public override string GetDescription()
        {
            return $"Target object is {_targetElement.name} and Expected state is {GetExpectedState()}";
        }
    }
}