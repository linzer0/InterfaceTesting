using InterfaceTesting.TestableElements.Base;
using UnityEngine;
using UnityEngine.Events;

namespace InterfaceTesting.TestableElements
{
    public class TestableWindow : UIElement
    {
        [SerializeField] private GameObject _content;

        public event UnityAction OnHide;
        public event UnityAction OnShow;

        protected override void Initialize()
        {
        }

        public override void Simulate()
        {
            Show();
        }

        public void Show()
        {
            _content.SetActive(true);
            OnShow?.Invoke();
        }

        public void Hide()
        {
            _content.SetActive(true);
            OnHide?.Invoke();
        }

        public void Test()
        {
        }
    }
}