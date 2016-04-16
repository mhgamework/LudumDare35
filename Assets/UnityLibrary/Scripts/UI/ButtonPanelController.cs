using System.Collections.Generic;
using Miscellaneous.ObjectPooling;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// A ui helper class for spawning/despawning a list of buttons.
    /// </summary>
    public class ButtonPanelController : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private ObjectPoolComponent buttonPool = null;
        [SerializeField]
        private Transform buttonContainerTransform = null;
        [SerializeField]
        private int buttonHeight = 30;

        private Transform buttonPoolTransform;
        private List<Button> currentPooledObjects = new List<Button>();

        // .. OPERATIONS

        /// <summary>
        /// Instantiate and display a new button.
        /// </summary>
        /// <returns></returns>
        public Button CreateButton()
        {
            if (buttonPoolTransform == null)
                buttonPoolTransform = buttonPool.GetComponent<Transform>();

            var new_object = buttonPool.FetchPooledObject();
            new_object.GetComponent<Transform>().SetParent(buttonContainerTransform);

            var layout_element = new_object.GetComponent<LayoutElement>();
            if (layout_element == null)
                layout_element = new_object.GameObject.AddComponent<LayoutElement>();
            layout_element.minHeight = buttonHeight;

            var button = new_object.GetComponent<Button>();
            new_object.GameObject.SetActive(true);

            currentPooledObjects.Add(button);

            return button;
        }

        /// <summary>
        /// Remove a currently instantiated button.
        /// </summary>
        /// <param name="button"></param>
        public void RemoveButton(Button button)
        {
            if (!currentPooledObjects.Contains(button))
                return;

            currentPooledObjects.Remove(button);
            button.gameObject.SetActive(false);
            buttonPool.ReleasPooledObjectFromGameObject(button.gameObject);
        }
    }
}
