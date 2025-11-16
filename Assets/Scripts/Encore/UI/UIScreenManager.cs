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

            if (!_screens.ContainsKey(UIScreenNames.MainMenu))
            {
                GameObject startObj = new("StartGameScreen") { hideFlags = HideFlags.DontSave };
                startObj.transform.SetParent(transform, false);
                startObj.SetActive(false);
                StartGameScreen start = startObj.AddComponent<StartGameScreen>();
                RegisterScreen(start);
            }

            if (_screens.ContainsKey(UIScreenNames.StatsScreen)) return;
            GameObject statsObj = new("StatsScreen") { hideFlags = HideFlags.DontSave };
            statsObj.transform.SetParent(transform, false);
            statsObj.SetActive(false);
            StatsScreen stats = statsObj.AddComponent<StatsScreen>();
            RegisterScreen(stats);

            bool hasProgress = GameManager?.Instance?.Events?.GetAllEvents() is { Count: > 0 };
            ShowScreen(hasProgress ? UIScreenNames.StatsScreen : UIScreenNames.MainMenu);

            _initialisedComplete = true;
        }

        private void HideAllScreens()
        {
            List<UIScreenBase> screens = new(_screens.Values);
            foreach (UIScreenBase screen in screens.Where(screen => screen.IsVisible()))
            {
                screen.SetVisible(false);
            }
        }
    }
}