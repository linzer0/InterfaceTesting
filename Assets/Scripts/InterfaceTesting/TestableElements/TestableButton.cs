using InterfaceTesting.TestableElements.Base;
using UnityEngine.UI;

namespace InterfaceTesting
{
    public class TestableButton : Button, ITestable
    {
        public void Simulate()
        {
            onClick.Invoke();
        }
    }
}