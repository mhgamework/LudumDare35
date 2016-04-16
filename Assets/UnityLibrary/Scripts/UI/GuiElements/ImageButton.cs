using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GuiElements
{
    public class ImageButton : MonoBehaviour, IPointerClickHandler
    {
        // .. ATTRIBUTES

        [SerializeField]
        private Image ImageComponent = null;
        [SerializeField]
        private Text TextComponent = null;
        [SerializeField]
        private Text AdditionTextComponent = null;
        [SerializeField]
        private AGuiElement GuiElement = null;

        private object associatedObject;

        // .. EVENTS

        [Serializable]
        public class ObjectUnityEvent : UnityEvent<object> { }
        public ObjectUnityEvent OnClickedEvent;

        // .. OPERATIONS

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickedEvent != null)
                OnClickedEvent.Invoke(associatedObject);
        }

        public Text GetText()
        {
            return TextComponent;
        }

        public Text GetAdditionalText()
        {
            return AdditionTextComponent;
        }

        public Image GetImage()
        {
            return ImageComponent;
        }

        public void SetAssociatedObject(object o)
        {
            associatedObject = o;
        }

        public object GetAssociatedObject()
        {
            return associatedObject;
        }

        public AGuiElement GetGuiElement()
        {
            return GuiElement;
        }


    }
}