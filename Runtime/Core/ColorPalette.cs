using System;
using System.Collections.Generic;
using UnityEngine;

namespace Broccollie.Core
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Broccollie/Core/ColorPalette")]
    public class ColorPalette : ScriptableObject
    {
        [SerializeField] private List<ColorPaletteData> colors = new();

        private Dictionary<string, Color> colorDictionary = new();

        private void Awake()
        {
            foreach (ColorPaletteData data in colors)
                colorDictionary.Add(data.Key, data.Color);
        }

        #region Public Functions
        public Color GetColor(string colorName)
        {
            if (colorName == null || !colorDictionary.ContainsKey(colorName))
            {
                Debug.LogWarning("[ ColorPalette ] Color not found in the palette: " + colorName);
                return Color.white; // Default color is white.
            }
            return colorDictionary[colorName];
        }

        #endregion
    }

    [Serializable]
    public struct ColorPaletteData
    {
        public string Key;
        public Color Color;
    }
}
