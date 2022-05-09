using UnityEngine;

namespace Assets.Scripts.InterfaceTesting.Tests
{
    public class ColorComparisonTest : ImageTest
    {
        [SerializeField] private Color _expectedImageColor;

        public override void RunTest()
        {
            if (_targetImage.color.Equals(_expectedImageColor))
            {
                InvokeResult(false);
            }
            else
            {
                InvokeResult(true, GetReport());
            }
        }

        public override string GetReport()
        {
            return
                $"{_targetImage.name} have {_targetImage.color.ToString()}\nExpected {_expectedImageColor.ToString()}";
        }
    }
}