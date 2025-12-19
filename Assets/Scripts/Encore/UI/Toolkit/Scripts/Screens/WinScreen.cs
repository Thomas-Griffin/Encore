using System.Collections.Generic;
using System.Linq;
using Encore.Abstractions.Interfaces;
using Encore.Systems.Core;
using Encore.Model.Game;
using UnityEngine;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class WinScreen : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UIManager uiManager;

        [SerializeField] private GameContext gameContext;
        private VisualElement _root;

        private VisualElement _winReasonsContainer;
        private Label _daysTakenLabel;
        private Button _mainMenuButton;

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>()?.rootVisualElement;
            
            _winReasonsContainer = _root?.Q<VisualElement>("winReasonsContainer");
            _daysTakenLabel = _root?.Q<Label>("daysTakenLabel");
            _mainMenuButton = _root?.Q<Button>("mainMenuButton");

            if (_mainMenuButton != null) _mainMenuButton.clicked += OnMainMenuClicked;

            PopulateWinReasons();
        }

        private void OnDisable()
        {
            if (_mainMenuButton != null) _mainMenuButton.clicked -= OnMainMenuClicked;
            _mainMenuButton = null;
            _winReasonsContainer = null;
            _daysTakenLabel = null;
            _root = null;
        }

        private void PopulateWinReasons()
        {
            if (_winReasonsContainer == null || !gameContext) return;

            _winReasonsContainer.Clear();

            List<WinReasons> reasons = gameContext.Session?.WinReasons;
            if (reasons == null || reasons.Count == 0)
            {
                Label none = new("No win reasons recorded.") { name = "winReason_none" };
                _winReasonsContainer.Add(none);
            }
            else {
                foreach (WinReasons reason in reasons.Distinct())
                {
                    string description = WinReasonExtensions.ToDescription(reason);
                    Label label = new(description) { name = $"winReason_{reason}" };
                    _winReasonsContainer.Add(label);
                }
            }
            
            IDayService dayService = gameContext.DayService;
            if (_daysTakenLabel != null && dayService != null)
            {
                _daysTakenLabel.text = $"Days taken: {dayService.CurrentDay} / {dayService.TotalDays}";
            }
        }

        private void OnMainMenuClicked()
        {
            uiManager.ShowScreen(ScreenNames.MainMenu);
        }
    }
}