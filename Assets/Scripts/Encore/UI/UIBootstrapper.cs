using UnityEngine;

namespace Encore.UI
{
    public static class UIBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void DrawMainUI()
        {
            GameObject gameObject = new("UserInterface");
            UIScreenManager screenManager = gameObject.AddComponent<UIScreenManager>();
            screenManager.EnsureInitialised();
            Object.DontDestroyOnLoad(gameObject);
        }
    }
}