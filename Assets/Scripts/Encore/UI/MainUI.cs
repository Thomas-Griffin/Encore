using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems;
using UnityEngine;

namespace Encore.UI
{
    public class MainUI : MonoBehaviour
    {
        public GameManager gameManager;

        public Color backgroundColor = Color.black;

        private Texture2D _backgroundTexture;

        private TextureCache _textureCache;
        private StatPresenter _presenter;

        private GUIStyle _labelStyle;
        private GUIStyle _valueStyle;

        private const int StatSpacing = 8;
        private const float UIMargin = 10f;
        private const float StatBarWidth = 100f;
        private const float StatBarHeight = 20f;
        private const int FontSize = 30;


        void Awake()
        {
            if (!gameManager)
            {
                // try to find an existing GameManager in the scene as a fallback
                gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();
                if (!gameManager)
                {
                    Debug.LogError(
                        "MainUI: No GameManager assigned and none found in scene. UI will be disabled until a GameManager is present.");
                }
            }

            _backgroundTexture = CreateTexture(backgroundColor);

            if (gameManager && gameManager.statManager)
            {
                _presenter = new StatPresenter(gameManager.statManager);
            }

            _textureCache = new TextureCache();
        }

        void Update()
        {
            _presenter?.Update(Time.deltaTime);
        }

        void OnGUI()
        {
            if (!gameManager || !gameManager.statManager) return;

            if (_labelStyle == null || _valueStyle == null)
            {
                _labelStyle = new GUIStyle(GUI.skin.label) { richText = false, fontSize = FontSize };
                _valueStyle = new GUIStyle(GUI.skin.label)
                    { alignment = TextAnchor.MiddleRight, fontSize = FontSize - 4 };
            }

            // Fullscreen panel
            Rect panelRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(panelRect, _backgroundTexture);

            Rect inner = new Rect(panelRect.x + UIMargin, panelRect.y + UIMargin,
                Mathf.Max(0, panelRect.width - 2f * UIMargin), Mathf.Max(0, panelRect.height - 2f * UIMargin));

            GUILayout.BeginArea(inner);
            GUILayout.Space(4);

            DrawStats();

            GUILayout.Space(12);

            DrawActionButtons();

            GUILayout.Space(8);
            DrawGameButtons();

            GUILayout.EndArea();
        }

        private void DrawStats()
        {
            var stats = gameManager?.statManager?.GetStats();
            if (stats == null) return;

            foreach (GameStat stat in stats)
            {
                if (stat == null) continue; // defensive: skip null stat entries
                DrawStat(stat);
                GUILayout.Space(StatSpacing);
            }
        }

        private void DrawStat(GameStat gameStat)
        {
            if (gameStat == null) return; // defensive: avoid NRE when called with a null stat

            float displayed = _presenter?.GetDisplayedValue(gameStat) ?? gameStat.CurrentValue;

            float statPercentage = Mathf.Clamp01(displayed / gameStat.MaxValue);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(gameStat.Stat.ToString(), _labelStyle);
            GUILayout.Label($"{Mathf.RoundToInt(displayed)}/{gameStat.MaxValue}", _valueStyle,
                GUILayout.Width(StatBarWidth));
            GUILayout.EndHorizontal();


            Rect statBar = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(StatBarHeight),
                GUILayout.ExpandWidth(true));

            GUI.DrawTexture(statBar, _backgroundTexture);

            float fillWidth = statPercentage > 0f ? Mathf.Max(1f, statBar.width * statPercentage) : 0f;
            Rect fillRect = new Rect(statBar.x, statBar.y, fillWidth, statBar.height);

            GUI.DrawTexture(fillRect, _textureCache.Get(gameStat.Colour));
            GUILayout.EndVertical();
        }

        private void DrawActionButtons()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(nameof(GameActions.Rest), GUILayout.Height(30)))
            {
                gameManager.DoAction(GameActions.Rest);
            }

            if (GUILayout.Button(nameof(GameActions.Practice), GUILayout.Height(30)))
            {
                gameManager.DoAction(GameActions.Practice);
            }

            if (GUILayout.Button(nameof(GameActions.Gig), GUILayout.Height(30)))
            {
                gameManager.DoAction(GameActions.Gig);
            }

            GUILayout.EndHorizontal();
        }

        private void DrawGameButtons()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Stats", GUILayout.Height(24)))
            {
                if (gameManager?.statManager)
                {
                    gameManager.statManager.ResetStats();
                }
                else
                {
                    Debug.LogWarning("MainUI: Cannot reset stats because StatManager is not available.");
                }
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

        private Texture2D CreateTexture(Color col)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, col);
            texture.Apply();
            texture.hideFlags = HideFlags.DontSave; // Prevent memory leaks
            return texture;
        }

        void OnDestroy()
        {
            if (_backgroundTexture) DestroyImmediate(_backgroundTexture);
            _textureCache.Clear();
            _presenter = null;
        }
    }
}