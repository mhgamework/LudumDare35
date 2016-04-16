using System;
using Miscellaneous.ObjectPooling;
using UI;
using UI.GuiElements;
using UnityEngine;
using UnityEngine.Events;

namespace Miscellaneous
{
    /// <summary>
    /// Helper class to switch between unity quality levels at runtime by pressing buttons.
    /// </summary>
    public class QualitySelector : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private RectTransform QualityButtonsPanel = null;
        [SerializeField]
        private ObjectPoolComponent ButtonPool = null;

        // .. TYPES

        [Serializable]
        public class OnQualityChangedEvent : UnityEvent { }
        public OnQualityChangedEvent OnQualityChanged;

        // .. INITIALIZATION

        void Start()
        {
            BuildButtons();
        }

        // .. OPERATIONS

        private void BuildButtons()
        {
            var quality_names = QualitySettings.names;
            for (int i = 0; i < quality_names.Length; i++)
            {
                var new_object = ButtonPool.FetchPooledObject().GameObject;
                new_object.SetActive(true);

                var button = new_object.GetComponent<BounceButton>();
                HookupButton(button, i);

                var text_setter = new_object.GetComponent<TextSetter>();
                text_setter.SetText(quality_names[i]);

                new_object.GetComponent<RectTransform>().SetParent(QualityButtonsPanel);
            }
        }

        private void HookupButton(BounceButton b, int index)
        {
            b.onClick.AddListener(() => SelectQuality(index));
        }

        private void SelectQuality(int index)
        {
            QualitySettings.SetQualityLevel(index, true);

            if (OnQualityChanged != null)
                OnQualityChanged.Invoke();
        }

    }
}
