﻿using UnityEngine;

namespace InterfaceTesting
{
    public class ImageComparisonTest : ImageTest
    {
        [SerializeField] private Sprite _expectedSprite;

        public override void RunTest()
        {
            if (SpritesAreEqual(_targetImage.sprite, _expectedSprite))
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
            return $"{_targetImage.name} have {_targetImage.sprite.name}\nExpected {_expectedSprite.name}";
        }

        private bool SpritesAreEqual(Sprite first, Sprite second)
        {
            return first.Equals(second);
        }

        public override string GetDescription()
        {
            return $"Target object is {_targetImage.name} Expected sprite is {_expectedSprite.name}";
        }
    }
}