using UnityEditor;
using UnityEngine;

namespace VRTools.Editor.VRRecorder
{
    internal class VRRecorderSettingsWindow : EditorWindow
    {
        void OnGUI()
        {
            GUILayout.Label("Session Name:", EditorStyles.boldLabel);
            GUI.enabled = true;
        }
    }
}