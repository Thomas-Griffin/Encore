using Encore.Systems;
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
            gameObject.AddComponent<GameManager>();
            gameObject.AddComponent<MainUI>();
            Object.DontDestroyOnLoad(gameObject);
        }
    }
}