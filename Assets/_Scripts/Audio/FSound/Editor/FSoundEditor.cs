using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
namespace Shoelace.Audio
{
    [CustomEditor(typeof(FSoundData)), CanEditMultipleObjects]
    public class FSoundEditor : Editor
    {
        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();

        //    FSoundData f = (FSoundData)target;

        //    if (f.selectSound.IsNull)
        //        return;

        //    if (GUILayout.Button("Get Event Parameters References"))
        //    {
        //        if(targets.Length == 1)
        //            f.GetAllParameters();
        //        else
        //        {
        //            for (int i = 0; i < targets.Length; i++)
        //            {
        //                FSoundData s = (FSoundData)targets[i];
        //                s.GetAllParameters();
        //            }
        //        }
        //    }

        //    if (f.parameters == null || f.parameters.Count == 0)
        //        return;

        //    for (int i = 0; i < f.parameters.Count; i++)
        //    {
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.LabelField("Parameter : ");
        //        EditorGUILayout.LabelField(f.parameters[i].name);
        //        EditorGUILayout.EndHorizontal();

        //        EditorGUILayout.LabelField("Min : " + f.parameters[i].minimum.ToString());
        //        EditorGUILayout.LabelField("Default : " + f.parameters[i].defaultvalue.ToString());
        //        EditorGUILayout.LabelField("Max : " + f.parameters[i].maximum);
        //    }

        //    if(GUILayout.Button("Clear Parameters"))
        //    {
        //        if (targets.Length == 1)
        //            f.parameters.Clear();
        //        else
        //        {
        //            for (int i = 0; i < targets.Length; i++)
        //            {
        //                FSoundData s = (FSoundData)targets[i];
        //                s.parameters.Clear();
        //            }
        //        }
        //    }
        //}
    }
}
#endif