using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "My Assets/Editor")]
[CustomEditor(typeof(Settings))]
public class SettingsEditor : Editor
{

    public int muNum = 199;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Settings settings = (Settings)target;



        if (GUILayout.Button("Reset"))
        {
            settings.Reset();
        }
    }


}
