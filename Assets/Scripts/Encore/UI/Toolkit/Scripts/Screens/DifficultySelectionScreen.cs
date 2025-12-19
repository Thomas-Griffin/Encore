using Encore.Model.Game;
using Encore.Systems.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class DifficultySelectionScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameContext gameContext;

        private VisualElement _root;

        private Button _easy;
        private Button _medium;
        private Button _hard;
        private Button _cancel;

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>()?.rootVisualElement;

            _easy = _root?.Q<Button>("easyButton");
            _medium = _root?.Q<Button>("mediumButton");
            _hard = _root?.Q<Button>("hardButton");
            _cancel = _root?.Q<Button>("cancelDifficulty");

            if (_easy != null) _easy.clicked += OnEasyClicked;
            if (_medium != null) _medium.clicked += OnMediumClicked;
            if (_hard != null) _hard.clicked += OnHardClicked;
            if (_cancel != null) _cancel.clicked += OnCancelClicked;
        }

        private void OnDisable()
        {
            if (_easy != null) _easy.clicked -= OnEasyClicked;
            if (_medium != null) _medium.clicked -= OnMediumClicked;
            if (_hard != null) _hard.clicked -= OnHardClicked;
            if (_cancel != null) _cancel.clicked -= OnCancelClicked;

            _easy = _medium = _hard = _cancel = null;
            _root = null;
        }

        private void OnCancelClicked()
        {
            uiManager.ShowScreen(ScreenNames.MainMenu);
        }

        private void OnHardClicked() => Select(Difficulty.Hard);

        private void OnMediumClicked() => Select(Difficulty.Medium);

        private void OnEasyClicked() => Select(Difficulty.Easy);

        private void Select(Difficulty difficulty)
        {
            gameContext.GameManager.StartGame(difficulty);
            uiManager.ShowScreen(ScreenNames.MainGame);
        }
    }
}