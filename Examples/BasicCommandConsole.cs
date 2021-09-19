using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CommandConsole.Examples
{
    public class BasicCommandConsole : CommandConsoleBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private RectTransform _outputContainer;

        [SerializeField] private ScrollRect _scrollRect;
        
        [SerializeField] private Text _output;

        [SerializeField] private InputField _inputField;

        public void OnSubmitButtonClicked()
        {
            Submit();
        }
        
        protected override void Submit()
        {
            HandleInput(_inputField.text);
            _inputField.text = ""; 
        }

        protected override void OnShow()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _inputField.ActivateInputField();
            _inputField.Select();
            EventSystem.current?.SetSelectedGameObject(_inputField.gameObject);
        }

        protected override void OnHide()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _inputField.DeactivateInputField();
        }

        public override void Print(string text)
        {
            _output.text += $"\n{text}\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate(_outputContainer);
            _scrollRect.normalizedPosition = new Vector2(0, 0);
        }

        public override void Clear()
        {
            _output.text = "";
            LayoutRebuilder.ForceRebuildLayoutImmediate(_outputContainer);
        }
    }
}