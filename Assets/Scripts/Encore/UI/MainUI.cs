using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.Core;
using UnityEngine;
using System.Collections.Generic;

namespace Encore.UI
{
    public class MainUI : MonoBehaviour
    {
        public GameManager gameManager;

        public Color backgroundColor = Color.black;

        private Texture2D _backgroundTexture;

        private TextureCache _textureCache;
        private StatPresenter _presenter;

        private readonly Dictionary<GameStats, GUIContent> _statNameContent = new();
        private readonly Dictionary<GameStats, GUIContent> _statValueContent = new();

        private GUIStyle _labelStyle;
        private GUIStyle _valueStyle;

        private const int StatSpacing = 8;
        private const float UIMargin = 10f;
        private const float StatBarWidth = 100f;
        private const float StatBarHeight = 20f;
        private const int FontSize = 30;


        private void Awake()
        {
            if (!gameManager)
            {
                gameManager = FindAnyObjectByType<GameManager>();
                if (!gameManager)
                {
                    Debug.LogError(
                        "MainUI: No GameManager assigned and none found in scene. UI will be disabled until a GameManager is present.");
                }
            }

            Initialise(gameManager);
        }

        public void Initialise(GameManager game)
        {
            gameManager = game ?? gameManager;

            if (_backgroundTexture == null)
            {
                _backgroundTexture = CreateTexture(backgroundColor);
            }

            _textureCache ??= new TextureCache();

            if (_presenter == null && gameManager?.Instance!?.Stats)
            {
                _presenter ??= new StatPresenter(gameManager?.Instance.Stats);
            }

            if (!gameManager?.Instance!?.Stats) return;
            foreach (GameStat stat in gameManager!.Instance?.Stats.GetStats()!)
            {
                if (stat == null) continue;
                if (!_statNameContent.ContainsKey(stat.Stat))
                    _statNameContent[stat.Stat] = new GUIContent(stat.Stat.ToString());
                if (!_statValueContent.ContainsKey(stat.Stat))
                    _statValueContent[stat.Stat] = new GUIContent($"{Mathf.RoundToInt(stat.LastValue)}/{stat.MaxValue}");
            }
        }

        private void Update()
        {
            _presenter?.Update(Time.deltaTime);
        }

        private void OnGUI()
        {
            if (!gameManager?.Instance!?.Stats) return;

            if (_labelStyle == null || _valueStyle == null)
            {
                _labelStyle = new GUIStyle(GUI.skin.label) { richText = false, fontSize = FontSize };
                _valueStyle = new GUIStyle(GUI.skin.label)
                    { alignment = TextAnchor.MiddleRight, fontSize = FontSize - 4 };
            }

            Rect panelRect = new(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(panelRect, _backgroundTexture);

            Rect screenRect = new(
                panelRect.x + UIMargin,
                panelRect.y + UIMargin,
                Mathf.Max(0, panelRect.width - 2f * UIMargin),
                Mathf.Max(0, panelRect.height - 2f * UIMargin)
            );
            GUILayout.BeginArea(screenRect);

            GUILayout.Space(4);

            DrawDayCounter();
            DrawStats();

            GUILayout.Space(12);

            DrawActionButtons();

            GUILayout.Space(8);

            DrawStartGameAndResetButtons();

            GUILayout.EndArea();
        }

        private void DrawDayCounter()
        {
            int currentDay = gameManager?.Instance?.Days?.CurrentDay ?? 0;
            int totalDays = gameManager?.Instance?.Days?.TotalDays ?? 0;
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Day: {currentDay} / {totalDays}", _labelStyle);
            GUILayout.EndHorizontal();
        }

        private void DrawStats()
        {
            GameStat[] stats = gameManager?.Instance?.Stats?.GetStats();
            if (stats == null) return;

            foreach (GameStat stat in stats)
            {
                if (stat == null) continue; 
                DrawStat(stat);
                GUILayout.Space(StatSpacing);
            }
        }

        private void DrawStat(GameStat gameStat)
        {
            if (gameStat == null) return;
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                if (!_statNameContent.TryGetValue(gameStat.Stat, out GUIContent nameContent))
                {
                    nameContent = new GUIContent(gameStat.Stat.ToString());
                    _statNameContent[gameStat.Stat] = nameContent;
                }
                GUILayout.Label(nameContent, _labelStyle);

                if (!_statValueContent.TryGetValue(gameStat.Stat, out GUIContent valueContent))
                {
                    valueContent = new GUIContent();
                    _statValueContent[gameStat.Stat] = valueContent;
                }
                string newValueText = $"{Mathf.RoundToInt(gameStat.LastValue)}/{gameStat.MaxValue}";
                if (valueContent.text != newValueText)
                {
                    valueContent.text = newValueText;
                }
                GUILayout.Label(valueContent, _valueStyle, GUILayout.Width(StatBarWidth));
                GUILayout.EndHorizontal();
                Rect statBar = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(StatBarHeight),
                    GUILayout.ExpandWidth(true));
                GUI.DrawTexture(statBar, _backgroundTexture);

                float statPercentage = gameStat.MaxValue == 0
                    ? 0f
                    : Mathf.Clamp01(gameStat.LastValue / (float)gameStat.MaxValue);
                float fillWidth = statPercentage > 0f ? Mathf.Max(1f, statBar.width * statPercentage) : 0f;
                Rect fillRect = new(statBar.x, statBar.y, fillWidth, statBar.height);

                GUI.DrawTexture(fillRect, _textureCache.Get(gameStat.Colour));
                GUILayout.EndVertical();
            }
        }

        private void DrawActionButtons()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(nameof(GameActions.Rest), GUILayout.Height(30)))
            {
                gameManager.DoAction(new SimpleGameAction(GameActions.Rest));
            }

            if (GUILayout.Button(nameof(GameActions.Practice), GUILayout.Height(30)))
            {
                gameManager.DoAction(new SimpleGameAction(GameActions.Practice));
            }

            if (GUILayout.Button(nameof(GameActions.Gig), GUILayout.Height(30)))
            {
                gameManager.DoAction(new SimpleGameAction(GameActions.Gig));
            }

            GUILayout.EndHorizontal();
        }

        private void DrawStartGameAndResetButtons()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Stats", GUILayout.Height(24)))
            {
                gameManager?.Instance?.Stats?.ResetStats();
            }

            if (GUILayout.Button("Start Game (Easy)", GUILayout.Height(24)))
            {
                gameManager?.StartGame(DifficultyLevel.Easy);
            }

            if (GUILayout.Button("Start Game (Medium)", GUILayout.Height(24)))
            {
                gameManager?.StartGame(DifficultyLevel.Medium);
            }

            if (GUILayout.Button("Start Game (Hard)", GUILayout.Height(24)))
            {
                gameManager?.StartGame(DifficultyLevel.Hard);
            }

            GUILayout.EndHorizontal();
        }

        private static Texture2D CreateTexture(Color col)
        {
            Texture2D texture = new(1, 1);
            texture.SetPixel(0, 0, col);
            texture.Apply();
            texture.hideFlags = HideFlags.DontSave;
            return texture;
        }

        private void OnDestroy()
        {
            if (_backgroundTexture) DestroyImmediate(_backgroundTexture);
            _textureCache?.Clear();
            _presenter = null;
        }
    }
}