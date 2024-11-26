#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(RadarChart), true)]
[CanEditMultipleObjects]
public class RadarChartEditor : ImageEditor
{
    SerializedProperty _pointCount;
    SerializedProperty _pointSprite;
    SerializedProperty _pointColor;
    SerializedProperty _pointSize;
    SerializedProperty _handlerRadio;

    protected override void OnEnable()
    {
        base.OnEnable();
        _pointCount = serializedObject.FindProperty("_pointCount");
        _pointSprite = serializedObject.FindProperty("_pointSprite");
        _pointColor = serializedObject.FindProperty("_pointColor");
        _pointSize = serializedObject.FindProperty("_pointSize");
        _handlerRadio = serializedObject.FindProperty("_handlerRadio");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(_pointCount);
        EditorGUILayout.PropertyField(_pointSprite);
        EditorGUILayout.PropertyField(_pointColor);
        EditorGUILayout.PropertyField(_pointSize);
        EditorGUILayout.PropertyField(_handlerRadio, true);

        RadarChart radar = target as RadarChart;
        if (radar != null)
        {
            if (GUILayout.Button("�����״�ͼ����"))
            {
                radar.InitPoint();
            }

            if (GUILayout.Button("�����ڲ��ɲ�������"))
            {
                radar.InitHandlers();
            }
        }
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

    }
}
#endif