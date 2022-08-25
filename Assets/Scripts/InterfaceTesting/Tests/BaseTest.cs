using UnityEngine;
using UnityEngine.Events;

namespace InterfaceTesting
{
    public class BaseTest : MonoBehaviour
    {
        public event UnityAction<string> OnTestFail;
        public event UnityAction OnTestCompleted;

        public virtual void RunTest()
        {
        }

        public void InvokeResult(bool testFailed, string report = "")
        {
            if (testFailed)
            {
                OnTestFail?.Invoke(report);
            }
            else
            {
                OnTestCompleted?.Invoke();
            }
        }

        public void PrintTest()
        {
            Debug.Log($"{name}");
        }

        public virtual string GetReport()
        {
            return string.Empty;
        }

        public virtual string GetDescription()
        {
            return "Need fill in test description";
        }
    }
}