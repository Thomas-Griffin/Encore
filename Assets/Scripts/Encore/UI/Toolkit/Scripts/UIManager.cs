using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts
{
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField] private UIDocument mainMenuScreen;
        [SerializeField] private UIDocument mainGameScreen;
        [SerializeField] private UIDocument difficultySelectionScreen;
        [SerializeField] private UIDocument winScreen;
        [SerializeField] private UIDocument loseScreen;

        private readonly Dictionary<ScreenNames, UIDocument> _screens = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _screens[ScreenNames.MainMenu] = mainMenuScreen;
            _screens[ScreenNames.DifficultySelection] = difficultySelectionScreen;
            _screens[ScreenNames.MainGame] = mainGameScreen;
            _screens[ScreenNames.WinScreen] = winScreen;
            _screens[ScreenNames.LoseScreen] = loseScreen;
        }

        private void Start()
        {
            ShowScreen(ScreenNames.MainMenu);
        }

        public void ShowScreen(ScreenNames screenName)
        {
            if (!_screens.TryGetValue(screenName, out UIDocument screen) || screen == null)
            {
                Debug.LogError($"UIManager: Screen '{screenName}' not found or not assigned.");
                return;
            }

            foreach (GameObject screenGameObject in from keyValuePair in _screens.ToList()
                     select keyValuePair.Value
                     into uiDocument
                     where uiDocument
                     let gameObject = uiDocument.gameObject
                     where gameObject
                     where !Equals(uiDocument, screen)
                     where gameObject.activeSelf
                     select gameObject)
            {
                screenGameObject.SetActive(false);
            }

            if (!screen.gameObject.activeSelf) screen.gameObject.SetActive(true);

            if (screen.rootVisualElement != null)
            {
                screen.rootVisualElement.style.display = DisplayStyle.Flex;
            }
        }
    }
}