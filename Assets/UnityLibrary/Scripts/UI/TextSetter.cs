using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextSetter : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Text TextComponent = null;

        // .. OPERATIONS

        public void SetText(string text)
        {
            TextComponent.text = text;
        }

        public void SetTextColor(Color c)
        {
            TextComponent.color = c;
        }

    }
}
