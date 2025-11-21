using UnityEngine;
using Encore.Systems.Core;

namespace Encore.UI.Screens
{
    public abstract class UIScreenBase : MonoBehaviour
    {
        [Tooltip("Unique name for this screen (used by UIScreenManager).")]
        public UIScreenNames screenName = UIScreenNames.Unnamed;

        protected GameManager GameManager;

        private readonly Color _backgroundColor = new(0f, 0f, 0f, 1f);
        protected Texture2D BackgroundTexture;

        private bool _visible;

        private void Initialise(GameManager manager)
        {
            GameManager = manager ?? GameManager;
        }

        protected virtual void Awake()
        {
            TryRegister();
        }

        protected virtual void OnDestroy()
        {
            TryUnregister();
        }

        private void TryRegister()
        {
            if (UIScreenManager.Instance)
            {
                UIScreenManager.Instance.RegisterScreen(this);
            }
        }

        private void TryUnregister()
        {
            if (UIScreenManager.Instance != null)
            {
                UIScreenManager.Instance.UnregisterScreen(this);
            }
        }

        public void SetVisible(bool visible)
        {
            if (_visible == visible) return;
            _visible = visible;
            if (_visible) OnShow();
            else OnHide();
        }

        public bool IsVisible() => _visible;


        public virtual void OnInitialise(GameManager game)
        {
            if (!BackgroundTexture)
            {
                BackgroundTexture = CreateTexture(_backgroundColor);
            }


            Initialise(game);
        }

        protected virtual void OnShow()
        {
            gameObject.SetActive(true);
            enabled = true;
        }

        protected virtual void OnHide()
        {
            enabled = false;
            gameObject.SetActive(false);
        }

        private static Texture2D CreateTexture(Color col)
        {
            Texture2D texture = new(1, 1);
            texture.SetPixel(0, 0, col);
            texture.Apply();
            texture.hideFlags = HideFlags.DontSave;
            return texture;
        }
    }
}