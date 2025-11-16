using Encore.Systems.Core;
using UnityEngine;

namespace Encore.UI
{
    public static class UIManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void DrawMainUI()
        {
            if (Object.FindFirstObjectByType<MainUI>()) return;

            GameObject gameObject = new("MainUI");
            GameManager gameManager = gameObject.AddComponent<GameManager>();
            MainUI mainUI = gameObject.AddComponent<MainUI>();
            mainUI.Initialise(gameManager);
            Object.DontDestroyOnLoad(gameObject);
        }
    }
}