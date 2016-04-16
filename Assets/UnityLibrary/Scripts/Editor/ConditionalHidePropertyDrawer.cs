using Miscellaneous;
using UnityEditor;
using UnityEngine;

namespace Assets.UnityLibrary.Scripts.Editor
{
    /// <summary>
    /// A proprty drawer that can show/hide/enable/disable inspector properties based on a bool property.
    /// source: http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
    /// 
    /// Usage: [ConditionalHide(string, bool)]
    /// - string: the name of the controlling bool property
    /// - bool: if true show/hide the drawer, if false (default) enable/disable the drawer
    /// 
    /// Caveats
    /// - List/arrays: only items of a list are hidden, the list itself is still visible (you can still change the size)
    /// - Doesn't work together with  propertydrawers that change how a property is being drawn (eg TextAreaAttibute)
    /// </summary>
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //get the attribute data
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            //check if the propery we want to draw should be enabled
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            //Enable/disable the property
            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;

            //Check if we should draw the property
            if (!condHAtt.HideInInspector || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            //Ensure that the next property that is being drawn uses the correct settings
            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            if (!condHAtt.HideInInspector || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                //The property is not being drawn
                //We want to undo the spacing added before and after the property
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;

            //Look for the sourcefield within the object that the property belongs to
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);
            if (sourcePropertyValue != null)
            {
                enabled = sourcePropertyValue.boolValue;
            }
            else
            {
                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
            }

            return enabled;
        }
    }
}
