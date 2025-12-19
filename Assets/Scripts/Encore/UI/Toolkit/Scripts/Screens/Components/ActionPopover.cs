using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Encore.UI.Toolkit.Scripts.Screens.Components
{
    public class ActionPopover : VisualElement
    {
        private readonly VisualElement _content;

        public ActionPopover()
        {
            name = "actionPopover";
            AddToClassList("action-popover");

            _content = new VisualElement();
            Add(_content);
        }

        public new void Clear()
        {
            _content.Clear();
        }

        public void PopulateDeltas(List<(int delta, string name)> deltas)
        {
            Clear();
            if (deltas == null || deltas.Count == 0) return;

            foreach ((int delta, string statName) in deltas)
            {
                VisualElement row = new();
                row.AddToClassList("popover-row");

                Label nameLabel = new(statName);
                nameLabel.AddToClassList("popover-name");
                row.Add(nameLabel);

                Label deltaLabel = new((delta > 0 ? "+" : "") + delta);
                deltaLabel.AddToClassList(delta >= 0 ? "delta-positive" : "delta-negative");
                row.Add(deltaLabel);

                _content.Add(row);
            }
        }

        public void PopulateDescription(string description)
        {
            Clear();
            Label label = new(description) { name = "actionPopoverLabel" };
            _content.Add(label);
        }

        public void SetPosition(float left, float top)
        {
            style.position = Position.Absolute;
            style.left = left;
            style.top = top;
            style.display = DisplayStyle.Flex;
        }
    }
}