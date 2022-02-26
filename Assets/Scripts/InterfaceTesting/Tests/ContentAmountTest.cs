using InterfaceTesting.Tests;
using UnityEngine;

namespace Assets.Scripts.InterfaceTesting.Tests
{
    public class ContentAmountTest : BaseTest
    {
        [SerializeField] private Transform _targetContentHolder;
        [SerializeField] private int _expectedContentAmount;

        private int _transformChildAmount;

        public override void RunTest()
        {
            _transformChildAmount = _targetContentHolder.childCount;
            if (_transformChildAmount.Equals(_expectedContentAmount))
            {
                InvokeResult(false);
            }
            else
            {
                InvokeResult(false, GetReport());
            }
        }

        public override string GetReport()
        {
            return
                $"{_targetContentHolder.name} child amount {_transformChildAmount}" +
                $"\nExpected {_expectedContentAmount}";
        }
    }
}