using UnityEngine;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuScreen : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UIManager uiManager;

        private VisualElement _root;

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>()?.rootVisualElement;

            Button startButton = _root?.Q<Button>("startButton");

            startButton?.RegisterCallback<ClickEvent>(OnStartClicked);
        }


        private void OnStartClicked(ClickEvent evt)
        {
            uiManager.ShowScreen(ScreenNames.DifficultySelection);
        }
    }
}