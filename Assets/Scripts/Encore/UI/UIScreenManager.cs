using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Encore.Systems.Core;
using Encore.UI.Screens;
using UnityEngine;

namespace Encore.UI
{
    public class UIScreenManager : MonoBehaviour
    {
        public static UIScreenManager Instance { get; private set; }

        private GameManager GameManager { get; set; }

        private readonly Dictionary<UIScreenNames, UIScreenBase> _screens = new();

        private bool _initialisedComplete;
        private UIScreenNames _currentScreen = UIScreenNames.Unnamed;

        public void RegisterScreen(UIScreenBase screen)
        {
            if (!screen) return;
            if (_screens.ContainsValue(screen))
            {
                return;
            }

            if (GameManager)
            {
                screen.OnInitialise(GameManager);
            }

            _screens[screen.screenName] = screen;
            if (!_initialisedComplete)
            {
                screen.SetVisible(false);
            }
            else
            {
                screen.SetVisible(screen.screenName == _currentScreen);
            }
        }

        public void UnregisterScreen(UIScreenBase screen)
        {
            if (!screen) return;
            _screens.Remove(screen.screenName);
        }

        public void ShowScreen(UIScreenNames screenName)
        {
            HideAllScreens();
            _currentScreen = screenName;
            if (_screens.TryGetValue(screenName, out UIScreenBase screen))
            {
                screen.SetVisible(true);
            }
        }

        public UIScreenBase GetScreen(UIScreenNames screenName)
        {
            return _screens.GetValueOrDefault(screenName);
        }

        private void Awake()
        {
            if (Instance && !Equals(Instance, this))
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            EnsureInitialised();
        }

        private IEnumerator Start()
        {
            yield return null;

            EnsureInitialised();

            bool hasProgress = GameManager?.Instance?.Events?.GetAllEvents() is { Count: > 0 };
            ShowScreen(hasProgress ? UIScreenNames.StatsScreen : UIScreenNames.MainMenu);
        }

        public void EnsureInitialised()
        {
            GameManager ??= FindAnyObjectByType<GameManager>() ?? gameObject.AddComponent<GameManager>();

            UIScreenBase[] foundScreens = FindObjectsByType<UIScreenBase>(FindObjectsSortMode.None);
            foreach (UIScreenBase screen in foundScreens)
            {
                RegisterScreen(screen);
            }

            InitialiseScreen(UIScreenNames.MainMenu);
            InitialiseScreen(UIScreenNames.StatsScreen);
            InitialiseScreen(UIScreenNames.WinScreen);
            InitialiseScreen(UIScreenNames.LoseScreen);

            bool hasProgress = GameManager?.Instance?.Events?.GetAllEvents() is { Count: > 0 };
            ShowScreen(hasProgress ? UIScreenNames.StatsScreen : UIScreenNames.MainMenu);

            _initialisedComplete = true;
        }

        private void InitialiseScreen(UIScreenNames screenName)
        {
            if (_screens.ContainsKey(screenName)) return;
            GameObject screenContainer = new(screenName.ToString()) { hideFlags = HideFlags.DontSave };
            screenContainer.transform.SetParent(transform, false);
            screenContainer.SetActive(false);
            UIScreenBase screen = screenName switch
            {
                UIScreenNames.MainMenu => screenContainer.AddComponent<StartGameScreen>(),
                UIScreenNames.StatsScreen => screenContainer.AddComponent<StatsScreen>(),
                UIScreenNames.WinScreen => screenContainer.AddComponent<WinScreen>(),
                UIScreenNames.LoseScreen => screenContainer.AddComponent<LoseScreen>(),
                _ => null
            };
            RegisterScreen(screen);
        }

        public void HideAllScreens()
        {
            List<UIScreenBase> screens = new(_screens.Values);
            foreach (UIScreenBase screen in screens.Where(screen => screen.IsVisible()))
            {
                screen.SetVisible(false);
            }
        }
    }
}