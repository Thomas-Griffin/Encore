using Encore.Systems.Core;
using Encore.Model.Game;
using System.Collections.Generic;
using System.Linq;
using Encore.Abstractions.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts.Screens
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class LoseScreen : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private UIManager uiManager;

        [SerializeField] private GameContext gameContext;

        private VisualElement _root;
        private VisualElement _loseReasonsContainer;
        private Button _mainMenuButton;

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>()?.rootVisualElement;

            if (gameContext == null) gameContext = GameContext.Instance;

            _loseReasonsContainer = _root?.Q<VisualElement>("loseReasonsContainer");
            _mainMenuButton = _root?.Q<Button>("mainMenuButton");

            if (_mainMenuButton != null) _mainMenuButton.clicked += OnMainMenuClicked;

            PopulateLoseReasons();
        }

        private void OnDisable()
        {
            if (_mainMenuButton != null) _mainMenuButton.clicked -= OnMainMenuClicked;
            _mainMenuButton = null;
            _loseReasonsContainer = null;
            _root = null;
        }

        private void PopulateLoseReasons()
        {
            if (_loseReasonsContainer == null || !gameContext) return;

            _loseReasonsContainer.Clear();

            List<LoseReasons> reasons = gameContext.Session?.LoseReasons;
            if (reasons == null || reasons.Count == 0)
            {
                Label none = new("No lose reasons recorded.") { name = "loseReason_none" };
                _loseReasonsContainer.Add(none);
            }
            else
            {
                foreach (Label label in from r in reasons
                         let desc = LoseReasonExtensions.ToDescription(r)
                         select new Label(desc) { name = $"loseReason_{r}" })
                {
                    _loseReasonsContainer.Add(label);
                }
            }

            IDayService ds = gameContext.DayService;
            if (ds == null) return;
            Label dayLabel = new Label($"Day: {ds.CurrentDay} / {ds.TotalDays}") { name = "daysTakenLabel" };
            _loseReasonsContainer.Add(dayLabel);
        }

        private void OnMainMenuClicked()
        {
            uiManager.ShowScreen(ScreenNames.MainMenu);
        }
    }
}