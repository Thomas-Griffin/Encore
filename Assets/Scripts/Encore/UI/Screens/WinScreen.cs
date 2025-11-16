using Encore.Systems.Core;
using Encore.Model.Game;
using UnityEngine;

namespace Encore.UI.Screens
{
    public class WinScreen : UIScreenBase
    {
        private GUIStyle _titleStyle;
        private GUIStyle _buttonStyle;
        private GUIStyle _labelStyle;

        public override void OnInitialise(GameManager game)
        {
            base.OnInitialise(game);
            screenName = UIScreenNames.WinScreen;
            gameManager = game ?? gameManager;
        }

        private void OnGUI()
        {
            if (!gameManager) return;
            if (!IsVisible()) return;

            _titleStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 56,
                fontStyle = FontStyle.Bold
            };

            _buttonStyle ??= new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                fixedHeight = 48,
                alignment = TextAnchor.MiddleCenter
            };

            _labelStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 42,
                fontStyle = FontStyle.Bold
            };

            Rect full = new(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(full, BackgroundTexture);

            float panelWidth = Mathf.Min(600, Screen.width * 0.7f);
            float panelHeight = 240f;
            Rect panel = new((Screen.width - panelWidth) * 0.5f, (Screen.height - panelHeight) * 0.4f, panelWidth,
                panelHeight);

            GUILayout.BeginArea(panel);
            GUILayout.FlexibleSpace();

            GUILayout.Label("You Win!", _titleStyle, GUILayout.Height(72));
            GUILayout.Space(20);

            DisplayWinReasons();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Main Menu", _buttonStyle, GUILayout.Width(180)))
            {
                UIScreenManager.Instance.ShowScreen(UIScreenNames.MainMenu);
            }

            GUILayout.Space(12);
            if (GUILayout.Button("Restart", _buttonStyle, GUILayout.Width(180)))
            {
                DifficultyLevel difficulty = gameManager?.Instance?.Difficulty ?? DifficultyLevel.Easy;
                gameManager?.StartGame(difficulty);
                UIScreenManager.Instance.ShowScreen(UIScreenNames.StatsScreen);
            }

            GUILayout.Space(12);
            if (GUILayout.Button("Quit", _buttonStyle, GUILayout.Width(140)))
            {
                Application.Quit();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }

        private void DisplayWinReasons()
        {
            foreach (WinReasons winReason in gameManager.Instance.LoseReasons)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{WinReasonExtensions.ToDescription(winReason)}");
                GUILayout.EndHorizontal();
            }
        }

        protected override void OnDestroy()
        {
            if (BackgroundTexture) DestroyImmediate(BackgroundTexture);
            base.OnDestroy();
        }
    }
}