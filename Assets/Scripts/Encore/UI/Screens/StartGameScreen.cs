using Encore.Model.Game;
using Encore.Systems.Core;
using UnityEngine;

namespace Encore.UI.Screens
{
    public class StartGameScreen : UIScreenBase
    {
        private GUIStyle _titleStyle;
        private GUIStyle _buttonStyle;
        private GUIStyle _labelStyle;

        public override void OnInitialise(GameManager game)
        {
            base.OnInitialise(game);
            screenName = UIScreenNames.MainMenu;
        }

        private void OnGUI()
        {
            if (!gameManager) return;
            if (!IsVisible()) return;

            if (_titleStyle == null || _buttonStyle == null)
            {
                _titleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 48,
                    fontStyle = FontStyle.Bold
                };
                
                _labelStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 32,
                    fontStyle = FontStyle.Normal
                };

                _buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 24,
                    fixedHeight = 48,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            Rect full = new(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(full, BackgroundTexture);

            float panelWidth = Mathf.Min(600, Screen.width * 0.8f);
            float panelHeight = 300f;
            Rect panel = new((Screen.width - panelWidth) * 0.5f, (Screen.height - panelHeight) * 0.4f, panelWidth,
                panelHeight);

            GUILayout.BeginArea(panel);
            GUILayout.FlexibleSpace();

            GUILayout.Label("Encore!", _titleStyle, GUILayout.Height(64));
            GUILayout.Space(20);
            
            GUILayout.Label("New Game:", _labelStyle, GUILayout.Height(64));
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Easy", _buttonStyle, GUILayout.Width(160)))
            {
                UIScreenManager.Instance.ShowScreen(UIScreenNames.StatsScreen);
                StartGame(DifficultyLevel.Easy);
            }

            GUILayout.Space(12);
            if (GUILayout.Button("Medium", _buttonStyle, GUILayout.Width(160)))
            {
                UIScreenManager.Instance.ShowScreen(UIScreenNames.StatsScreen);
                StartGame(DifficultyLevel.Medium);
            }

            GUILayout.Space(12);
            if (GUILayout.Button("Hard", _buttonStyle, GUILayout.Width(160)))
            {
                UIScreenManager.Instance.ShowScreen(UIScreenNames.StatsScreen);
                StartGame(DifficultyLevel.Hard);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Quit", _buttonStyle, GUILayout.Width(140)))
            {
                UIScreenManager.Instance.HideAllScreens();
                Application.Quit();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void StartGame(DifficultyLevel level)
        {
            if (!gameManager)
            {
                gameManager = FindAnyObjectByType<GameManager>();
            }

            gameManager?.StartGame(level);
        }
    }
}