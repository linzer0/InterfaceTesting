﻿using TMPro;
using UnityEngine;

namespace InterfaceTesting
{
    public class TextComponentTest : BaseTest
    {
        [SerializeField] private TextMeshProUGUI _textComponent;
        [SerializeField] private string _expectedValue;

        public override void RunTest()
        {
            if (_textComponent.text.Equals(_expectedValue))
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
            return $"{_textComponent.name} have {_textComponent.text}\nExpected {_expectedValue}";
        }

        public override string GetDescription()
        {
            return $"Target object is {_textComponent.name} and Expected value is {_expectedValue}";
        }
    }
}