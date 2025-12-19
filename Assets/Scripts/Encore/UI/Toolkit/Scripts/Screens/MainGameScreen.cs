using System;
using System.Collections.Generic;
using Encore.Abstractions.Interfaces;
using Encore.Model.Player;
using Encore.Model.Player.Actions;
using Encore.Model.Stats;
using Encore.Systems.Core;
using Encore.Systems.GameEvent;
using Encore.Systems.GameEvent.Events;
using Encore.UI.Toolkit.Scripts.Screens.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainGameScreen : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UIManager uiManager;

        [SerializeField] private GameContext gameContext;

        private VisualElement _root;
        private ProgressBar _energyBar;
        private ProgressBar _skillBar;
        private ProgressBar _popularityBar;
        private ProgressBar _fameBar;
        private Label _dayLabel;

        private Button _restButton;
        private Button _practiseButton;
        private Button _gigButton;

        private ActionPopover _actionPopover;
        private Button _popoverTargetButton;
        private PlayerAction _popoverAction;

        private EventCallback<PointerEnterEvent> _restEnter;
        private EventCallback<PointerLeaveEvent> _restLeave;
        private EventCallback<PointerEnterEvent> _practiseEnter;
        private EventCallback<PointerLeaveEvent> _practiseLeave;
        private EventCallback<PointerEnterEvent> _gigEnter;
        private EventCallback<PointerLeaveEvent> _gigLeave;

        private GameStatsAnimator _gameStatsAnimator;

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>()?.rootVisualElement;

            _energyBar = _root?.Q<ProgressBar>("energyBar");
            _skillBar = _root?.Q<ProgressBar>("skillBar");
            _popularityBar = _root?.Q<ProgressBar>("popularityBar");
            _fameBar = _root?.Q<ProgressBar>("fameBar");
            _dayLabel = _root?.Q<Label>("dayLabel");

            if (_energyBar != null)
            {
                _energyBar.lowValue = 0;
                _energyBar.highValue = 1;
            }

            if (_skillBar != null)
            {
                _skillBar.lowValue = 0;
                _skillBar.highValue = 1;
            }

            if (_popularityBar != null)
            {
                _popularityBar.lowValue = 0;
                _popularityBar.highValue = 1;
            }

            if (_fameBar != null)
            {
                _fameBar.lowValue = 0;
                _fameBar.highValue = 1;
            }

            _restButton = _root?.Q<Button>("restButton");
            _practiseButton = _root?.Q<Button>("practiseButton");
            _gigButton = _root?.Q<Button>("gigButton");

            try
            {
                if (_restButton != null)
                    _restButton.tooltip = GenerateTooltipForAction(new Rest());
                if (_practiseButton != null)
                    _practiseButton.tooltip = GenerateTooltipForAction(new Practice());
                if (_gigButton != null)
                    _gigButton.tooltip = GenerateTooltipForAction(new Gig());
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to generate action tooltips: {ex}");
            }

            if (_restButton != null)
            {
                _restEnter = OnRestPointerEnter;
                _restLeave = OnRestPointerLeave;
                _restButton.RegisterCallback(_restEnter);
                _restButton.RegisterCallback(_restLeave);
            }

            if (_practiseButton != null)
            {
                _practiseEnter = OnPractisePointerEnter;
                _practiseLeave = OnPractisePointerLeave;
                _practiseButton.RegisterCallback(_practiseEnter);
                _practiseButton.RegisterCallback(_practiseLeave);
            }

            if (_gigButton != null)
            {
                _gigEnter = OnGigPointerEnter;
                _gigLeave = OnGigPointerLeave;
                _gigButton.RegisterCallback(_gigEnter);
                _gigButton.RegisterCallback(_gigLeave);
            }

            if (_restButton != null) _restButton.clicked += OnRestClicked;
            if (_practiseButton != null) _practiseButton.clicked += OnPractiseClicked;
            if (_gigButton != null) _gigButton.clicked += OnGigClicked;

            if (gameContext)
            {
                _gameStatsAnimator = new GameStatsAnimator(gameContext.Stats);
            }
            else
            {
                Debug.LogWarning("MainGameScreenController: GameContext not found; stats will not update.");
            }

            RefreshAllStats();
            RefreshDayLabel();
            UpdateActionPreviews();
        }

        private void OnDisable()
        {
            if (_restButton != null) _restButton.clicked -= OnRestClicked;
            if (_practiseButton != null) _practiseButton.clicked -= OnPractiseClicked;
            if (_gigButton != null) _gigButton.clicked -= OnGigClicked;

            if (_restButton != null)
            {
                if (_restEnter != null) _restButton.UnregisterCallback(_restEnter);
                if (_restLeave != null) _restButton.UnregisterCallback(_restLeave);
            }

            if (_practiseButton != null)
            {
                if (_practiseEnter != null) _practiseButton.UnregisterCallback(_practiseEnter);
                if (_practiseLeave != null) _practiseButton.UnregisterCallback(_practiseLeave);
            }

            if (_gigButton != null)
            {
                if (_gigEnter != null) _gigButton.UnregisterCallback(_gigEnter);
                if (_gigLeave != null) _gigButton.UnregisterCallback(_gigLeave);
            }

            _restButton = _practiseButton = _gigButton = null;
            _energyBar = _skillBar = _popularityBar = _fameBar = null;
            _dayLabel = null;
            _gameStatsAnimator = null;

            HideActionPopover();
        }

        private void Update()
        {
            if (_gameStatsAnimator == null) return;
            _gameStatsAnimator.Update(Time.deltaTime);
            RefreshAllStats();
        }

        private void OnRestClicked()
        {
            gameContext.GameManager.DoAction(new Rest());
            ShowWinOrLoseScreen();
            UpdateActionPreviews();
        }

        private void OnPractiseClicked()
        {
            gameContext.GameManager.DoAction(new Practice());
            ShowWinOrLoseScreen();
            UpdateActionPreviews();
        }

        private void OnGigClicked()
        {
            gameContext.GameManager.DoAction(new Gig());
            ShowWinOrLoseScreen();
            UpdateActionPreviews();
        }

        private void ShowWinOrLoseScreen()
        {
            if (gameContext.Session.LoseReasons.Count > 0)
            {
                uiManager.ShowScreen(ScreenNames.LoseScreen);
            }
            else if (gameContext.Session.WinReasons.Count > 0)
            {
                uiManager.ShowScreen(ScreenNames.WinScreen);
            }
        }

        private void RefreshAllStats()
        {
            GameStat[] stats = gameContext.Stats?.GetStats();
            if (stats == null) return;
            foreach (GameStat stat in stats)
            {
                if (stat == null) continue;
                float normalized = stat.MaxValue <= 0 ? 0f : Mathf.Clamp01(stat.DisplayValue / stat.MaxValue);
                switch (stat.Stat)
                {
                    case GameStats.Energy:
                        SetStatFill(_energyBar, normalized);
                        break;
                    case GameStats.Skill:
                        SetStatFill(_skillBar, normalized);
                        break;
                    case GameStats.Popularity:
                        SetStatFill(_popularityBar, normalized);
                        break;
                    case GameStats.Fame:
                        SetStatFill(_fameBar, normalized);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RefreshDayLabel();
        }

        private void RefreshDayLabel()
        {
            if (_dayLabel == null || !gameContext) return;
            IDayService dayService = gameContext.DayService;
            if (dayService == null) return;
            _dayLabel.text = $"Day {dayService.CurrentDay} / {dayService.TotalDays}";
        }

        private static void SetStatFill(ProgressBar bar, float normalized)
        {
            if (bar == null)
            {
                return;
            }

            normalized = Mathf.Clamp01(normalized);
            float low = bar.lowValue;
            float high = bar.highValue;
            bar.value = Mathf.Lerp(low, high, normalized);
        }

        private string GenerateTooltipForAction(PlayerAction action)
        {
            if (action == null || gameContext == null) return string.Empty;
            GameEventBase evt = action.ToGameEvent();

            StatDeltas deltas = evt.PreviewDeltas(gameContext.Session, gameContext.Stats, gameContext.DayService) ??
                                new StatDeltas();

            List<string> parts = new();
            if (deltas.energyDelta != 0) parts.Add(FormatDelta(deltas.energyDelta, "Energy"));
            if (deltas.skillDelta != 0) parts.Add(FormatDelta(deltas.skillDelta, "Skill"));
            if (deltas.popularityDelta != 0) parts.Add(FormatDelta(deltas.popularityDelta, "Popularity"));
            if (deltas.fameDelta != 0) parts.Add(FormatDelta(deltas.fameDelta, "Fame"));

            string desc = evt.Description;
            string body = parts.Count > 0 ? string.Join(", ", parts) : desc;
            return body;
        }

        private static string FormatDelta(int delta, string name)
        {
            return delta > 0 ? $"+{delta} {name}" : $"{delta} {name}";
        }

        private void ShowActionPopover(Button button, PlayerAction action)
        {
            if (_root == null || button == null) return;

            _actionPopover ??= new ActionPopover();

            _popoverTargetButton = button;
            _popoverAction = action;

            _actionPopover.Clear();
            List<(int delta, string name)> deltas = GetActionDeltas(action);
            if (deltas.Count > 0)
            {
                _actionPopover.PopulateDeltas(deltas);
            }
            else
            {
                _actionPopover.PopulateDescription(action?.ToGameEvent()?.Description ?? string.Empty);
            }

            if (_actionPopover.parent == null)
                _root.Add(_actionPopover);

            Rect rect = button.worldBound;
            float posLeft = rect.x + rect.width * 0.5f - 100f;
            float top = rect.y + rect.height + 8f;

            float maxRight = _root.layout.width - 8f;
            if (posLeft < 8f) posLeft = 8f;
            if (posLeft + 200f > maxRight) posLeft = Math.Max(8f, maxRight - 200f);

            _actionPopover.SetPosition(posLeft, top);
        }

        private void HideActionPopover()
        {
            if (_actionPopover == null) return;
            _actionPopover.parent?.Remove(_actionPopover);
            _actionPopover = null;
            _popoverTargetButton = null;
            _popoverAction = null;
        }

        private void OnRestPointerEnter(PointerEnterEvent evt)
        {
            ShowActionPopover(_restButton, new Rest());
        }

        private void OnRestPointerLeave(PointerLeaveEvent evt)
        {
            HideActionPopover();
        }

        private void OnPractisePointerEnter(PointerEnterEvent evt)
        {
            ShowActionPopover(_practiseButton, new Practice());
        }

        private void OnPractisePointerLeave(PointerLeaveEvent evt)
        {
            HideActionPopover();
        }

        private void OnGigPointerEnter(PointerEnterEvent evt)
        {
            ShowActionPopover(_gigButton, new Gig());
        }

        private void OnGigPointerLeave(PointerLeaveEvent evt)
        {
            HideActionPopover();
        }

        private List<(int delta, string name)> GetActionDeltas(PlayerAction action)
        {
            List<(int delta, string name)> list = new List<(int delta, string name)>();
            if (action == null || gameContext == null) return list;
            GameEventBase gameEvent = action.ToGameEvent();
            StatDeltas deltas =
                gameEvent.PreviewDeltas(gameContext.Session, gameContext.Stats, gameContext.DayService) ??
                new StatDeltas();

            if (deltas.energyDelta != 0) list.Add((deltas.energyDelta, "Energy"));
            if (deltas.skillDelta != 0) list.Add((deltas.skillDelta, "Skill"));
            if (deltas.popularityDelta != 0) list.Add((deltas.popularityDelta, "Popularity"));
            if (deltas.fameDelta != 0) list.Add((deltas.fameDelta, "Fame"));

            return list;
        }


        private void UpdateActionPreviews()
        {
            try
            {
                if (_restButton != null) _restButton.tooltip = GenerateTooltipForAction(new Rest());
                if (_practiseButton != null) _practiseButton.tooltip = GenerateTooltipForAction(new Practice());
                if (_gigButton != null) _gigButton.tooltip = GenerateTooltipForAction(new Gig());

                _restButton?.SetEnabled(ActionRequirementsMet(new Rest()));
                _practiseButton?.SetEnabled(ActionRequirementsMet(new Practice()));
                _gigButton?.SetEnabled(ActionRequirementsMet(new Gig()));

                if (_actionPopover?.parent == null || _popoverAction == null ||
                    _popoverTargetButton == null) return;
                _actionPopover.Clear();
                List<(int delta, string name)> deltas = GetActionDeltas(_popoverAction);
                if (deltas.Count > 0)
                    _actionPopover.PopulateDeltas(deltas);
                else
                    _actionPopover.PopulateDescription(_popoverAction.ToGameEvent()?.Description ?? string.Empty);

                Rect worldBound = _popoverTargetButton.worldBound;
                float positionLeft = worldBound.x + worldBound.width * 0.5f - 100f;
                float top = worldBound.y + worldBound.height + 8f;
                float maxRight = _root.layout.width - 8f;
                if (positionLeft < 8f) positionLeft = 8f;
                if (positionLeft + 200f > maxRight) positionLeft = Math.Max(8f, maxRight - 200f);
                _actionPopover.SetPosition(positionLeft, top);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"UpdateActionPreviews failed: {exception}");
            }
        }

        private bool ActionRequirementsMet(PlayerAction action)
        {
            if (action == null || !gameContext) return false;
            GameEventBase gameEvent = action.ToGameEvent();
            return gameEvent?.Requirements != null && gameEvent.Requirements.AreMetBy(gameContext.Session, gameContext.Stats);
        }
    }
}