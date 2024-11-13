using UnityEditor;
using UnityEngine;

namespace Animation.Editor
{
	[CustomPropertyDrawer(typeof(ShoelaceAnimationEvent))]
	public class AnimationEventDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			SerializedProperty eventNameProperty = property.FindPropertyRelative("EventName");
			SerializedProperty eventProperty = property.FindPropertyRelative("OnAnimationEvent");

			Rect eventNameRect = new(
				position.x,
				position.y,
				position.width,
				EditorGUIUtility.singleLineHeight
			);

			Rect eventRect = new(
				position.x,
				position.y + EditorGUIUtility.singleLineHeight + 2,
				position.width,
				EditorGUI.GetPropertyHeight(eventProperty)
			);

			EditorGUI.PropertyField(eventNameRect, eventNameProperty);
			EditorGUI.PropertyField(eventRect, eventProperty, true);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty eventProperty = property.FindPropertyRelative("OnAnimationEvent");
			return EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(eventProperty) + 4;
		}
	}
}