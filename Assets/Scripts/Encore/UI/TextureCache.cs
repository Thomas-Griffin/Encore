using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Encore.UI
{
    public class TextureCache
    {
        private readonly Dictionary<Color, Texture2D> _cache = new();

        public Texture2D Get(Color color)
        {
            if (_cache.TryGetValue(color, out Texture2D texture) && texture) return texture;
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            texture.hideFlags = HideFlags.DontSave;
            _cache[color] = texture;
            return texture;
        }

        public void Clear()
        {
            foreach (KeyValuePair<Color, Texture2D> keyValuePair in _cache.Where(keyValuePair => keyValuePair.Value))
            {
                Object.DestroyImmediate(keyValuePair.Value);
            }

            _cache.Clear();
        }
    }
}

