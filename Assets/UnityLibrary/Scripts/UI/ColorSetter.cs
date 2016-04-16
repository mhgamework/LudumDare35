using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Helper class for altering the color of an Image component. Provides functionality to toggle between two different colors.
    /// </summary>
    public class ColorSetter : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Image ImageComponent = null;
        [SerializeField]
        private Color ToggleColorA = Color.white;
        [SerializeField]
        private Color ToggleColorB = Color.black;

        private Color CurrentToggleColor;

        // .. INITIALIZATION

        void Start()
        {
            CurrentToggleColor = ToggleColorA;
        }

        // .. OPERATIONS

        public void SetColor(Color c)
        {
            ImageComponent.color = c;
        }

        public void ToggleColor()
        {
            if (CurrentToggleColor == ToggleColorA)
                CurrentToggleColor = ToggleColorB;
            else
                CurrentToggleColor = ToggleColorA;

            SetColor(CurrentToggleColor);
        }
        
    }
}
