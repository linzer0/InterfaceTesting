using Assets.Scripts.InterfaceTesting.TestableElements.Base;
using UnityEngine.UI;

namespace InterfaceTesting.TestableElements
{
    public class TestableButton : Button, ITestable
    {
        public void Simulate()
        {
            onClick.Invoke();
        }
    }
}