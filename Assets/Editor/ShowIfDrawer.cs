using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty;

        SerializedProperty parent = property.Copy();
        if (parent.propertyPath.Contains("Array")) {
            string parentPath = property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.'));
            conditionProperty = property.serializedObject.FindProperty(parentPath + "." + showIf.conditionField);
        }
        else conditionProperty = property.serializedObject.FindProperty(showIf.conditionField);

        if (conditionProperty != null) {
            bool conditionMet = conditionProperty.boolValue;
            if (showIf.invertCondition) conditionMet = !conditionMet; 

            if (conditionMet) {
                EditorGUI.PropertyField(position, property, label);
            }
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty;

        SerializedProperty parent = property.Copy();
        if (parent.propertyPath.Contains("Array")) {
            string parentPath = property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.'));
            conditionProperty = property.serializedObject.FindProperty(parentPath + "." + showIf.conditionField);
        }
        else conditionProperty = property.serializedObject.FindProperty(showIf.conditionField);

        if (conditionProperty != null) {
            bool conditionMet = conditionProperty.boolValue;
            if (showIf.invertCondition) conditionMet = !conditionMet;

            return conditionMet ? EditorGUI.GetPropertyHeight(property, label) : 0;
        }

        return 0;
    }
}
