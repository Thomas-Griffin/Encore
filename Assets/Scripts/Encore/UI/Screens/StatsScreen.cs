using System.Collections.Generic;
using Encore.Model.Game;
using Encore.Model.Stats;
using Encore.Systems.Core;
using Encore.UI.Helpers;
using UnityEngine;

namespace Encore.UI.Screens
{
    public class StatsScreen : UIScreenBase
    {
        public override void OnInitialise(GameManager game)
        {
            base.OnInitialise(game);
            screenName = UIScreenNames.StatsScreen;
            GameManager = game ?? GameManager;

            _textureCache ??= new TextureCache();

            if (_presenter == null && GameManager?.Instance!?.Stats)
            {
                _presenter ??= new StatPresenter(GameManager?.Instance.Stats);
            }

            if (!GameManager?.Instance!?.Stats) return;
            foreach (GameStat stat in GameManager!.Instance?.Stats.GetStats()!)
            {
                if (stat == null) continue;
                if (!_statNameContent.ContainsKey(stat.Stat))
                    _statNameContent[stat.Stat] = new GUIContent(stat.Stat.ToString());
                if (!_statValueContent.ContainsKey(stat.Stat))
                    _statValueContent[stat.Stat] =
                        new GUIContent($"{Mathf.RoundToInt(stat.LastValue)}/{stat.MaxValue}");
            }
        }

        private TextureCache _textureCache;
        private StatPresenter _presenter;

        private readonly Dictionary<GameStats, GUIContent> _statNameContent = new();
        private readonly Dictionary<GameStats, GUIContent> _statValueContent = new();

        private GUIStyle _labelStyle;
        private GUIStyle _valueStyle;

        private GUIStyle _barLeftStyle;
        private GUIStyle _barRightStyle;

        private const int StatSpacing = 8;
        private const float UIMargin = 10f;
        private const float StatBarHeight = 36f;
        private const int FontSize = 30;

        private void Update()
        {
            _presenter?.Update(Time.deltaTime);
        }

        private void OnGUI()
        {
            if (!IsVisible()) return;

            if (!GameManager?.Instance!?.Stats) return;

            if (_labelStyle == null || _valueStyle == null)
            {
                _labelStyle = new GUIStyle(GUI.skin.label)
                    { richText = false, fontSize = FontSize, fontStyle = FontStyle.Bold };
                _valueStyle = new GUIStyle(GUI.skin.label)
                    { alignment = TextAnchor.MiddleRight, fontSize = FontSize - 4, fontStyle = FontStyle.Bold };
                int barFontSize = Mathf.Clamp((int)StatBarHeight - 2, 10, FontSize);
                _barLeftStyle = new GUIStyle(GUI.skin.label)
                    { alignment = TextAnchor.MiddleLeft, fontSize = barFontSize, fontStyle = FontStyle.Bold };
                _barRightStyle = new GUIStyle(GUI.skin.label)
                    { alignment = TextAnchor.MiddleRight, fontSize = barFontSize, fontStyle = FontStyle.Bold };
            }

            Rect panelRect = new(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(panelRect, BackgroundTexture);

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

            DrawMainMenuButton();

            GUILayout.EndArea();
        }

        private void DrawDayCounter()
        {
            int currentDay = GameManager?.Instance?.Days?.CurrentDay ?? 0;
            int totalDays = GameManager?.Instance?.Days?.TotalDays ?? 0;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label($"DAY: {currentDay} / {totalDays}", _labelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawStats()
        {
            GameStat[] stats = GameManager?.Instance?.Stats?.GetStats();
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
            GUILayout.BeginVertical();

            if (!_statNameContent.TryGetValue(gameStat.Stat, out GUIContent nameContent))
            {
                nameContent = new GUIContent(gameStat.Stat.ToString());
                _statNameContent[gameStat.Stat] = nameContent;
            }

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

            Rect statBar = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(StatBarHeight),
                GUILayout.ExpandWidth(true));
            GUI.DrawTexture(statBar, BackgroundTexture);

            float statPercentage = gameStat.MaxValue == 0
                ? 0f
                : Mathf.Clamp01(gameStat.LastValue / (float)gameStat.MaxValue);
            float fillWidth = statPercentage > 0f ? Mathf.Max(1f, statBar.width * statPercentage) : 0f;
            Rect fillRect = new(statBar.x, statBar.y, fillWidth, statBar.height);
            GUI.DrawTexture(fillRect, _textureCache.Get(gameStat.Colour));

            Color prevGuiColor = GUI.color;
            GUI.color = Color.white;
            float outlineThickness = 2f;
            GUI.DrawTexture(new Rect(statBar.x, statBar.y, statBar.width, outlineThickness), Texture2D.whiteTexture);
            GUI.DrawTexture(
                new Rect(statBar.x, statBar.y + statBar.height - outlineThickness, statBar.width, outlineThickness),
                Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(statBar.x, statBar.y, outlineThickness, statBar.height), Texture2D.whiteTexture);
            GUI.DrawTexture(
                new Rect(statBar.x + statBar.width - outlineThickness, statBar.y, outlineThickness, statBar.height),
                Texture2D.whiteTexture);
            GUI.color = prevGuiColor;

            float padding = 6f;
            Rect nameRect = new(statBar.x + padding, statBar.y, statBar.width * 0.6f - padding, statBar.height);
            Rect valueRect = new(statBar.x + statBar.width * 0.6f, statBar.y, statBar.width * 0.4f - padding,
                statBar.height);

            _barLeftStyle.normal.textColor = Color.white;
            _barRightStyle.normal.textColor = Color.white;

            GUI.Label(nameRect, nameContent, _barLeftStyle);
            GUI.Label(valueRect, valueContent, _barRightStyle);

            GUILayout.EndVertical();
        }

        private void DrawActionButtons()
        {
            GUILayout.BeginHorizontal();
            Color prevBg = GUI.backgroundColor;

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(nameof(GameActions.Rest), GUILayout.Height(36), GUILayout.ExpandWidth(true)))
            {
                GameManager.DoAction(new SimpleGameAction(GameActions.Rest));
            }

            GUI.backgroundColor = prevBg;

            GUILayout.Space(6);

            prevBg = GUI.backgroundColor;
            GUI.backgroundColor = new Color(1f, 0.6f, 0f);
            if (GUILayout.Button(nameof(GameActions.Practice), GUILayout.Height(36), GUILayout.ExpandWidth(true)))
            {
                GameManager.DoAction(new SimpleGameAction(GameActions.Practice));
            }

            GUI.backgroundColor = prevBg;

            GUILayout.Space(6);

            prevBg = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button(nameof(GameActions.Gig), GUILayout.Height(36), GUILayout.ExpandWidth(true)))
            {
                GameManager.DoAction(new SimpleGameAction(GameActions.Gig));
            }

            GUI.backgroundColor = prevBg;

            GUILayout.EndHorizontal();
        }

        private void DrawMainMenuButton()
        {
            float buttonWidth = 120f;
            float buttonHeight = 28f;
            float margin = 8f;
            float localX = Mathf.Max(0f, Screen.width - (UIMargin * 2f) - margin - buttonWidth);
            float localY = Mathf.Max(0f, margin);

            Rect buttonRect = new(localX, localY, buttonWidth, buttonHeight);
            if (GUI.Button(buttonRect, "Main Menu"))
            {
                UIScreenManager.Instance.ShowScreen(UIScreenNames.MainMenu);
            }
        }

        protected override void OnDestroy()
        {
            _textureCache?.Clear();
            _presenter = null;
            base.OnDestroy();
        }
    }
}