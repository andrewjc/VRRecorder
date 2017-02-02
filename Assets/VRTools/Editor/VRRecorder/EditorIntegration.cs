using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VRTools.Recorder;

namespace VRTools.Editor.VRRecorder
{
	[InitializeOnLoad]
	public class EditorIntegration
	{
		[MenuItem("VR Tools/Start Recording")]
		private static void StartRecording()
		{
		    if (VRPlayer.SessionPath == null || VRPlayer.SessionPath.Trim().Equals(""))
		    {
                EditorUtility.DisplayDialog("VR Recorder",
                "No session is set. Please set a session with VR Tools / Set Session Name",
                "OK");
		        return;
		    }

		    if (EditorApplication.isPlaying == false)
		    {
                EditorUtility.DisplayDialog("VR Recorder",
                "Game must be playing before starting recording.",
                "OK");
                return;
            }

			EditorUtility.DisplayDialog("Recorder Mode",
				"VR Recorder Enabled",
				"OK");
            GameObject.Find("[CameraRig]").GetComponent<VRPlayer>().StartRecording();
        }

        [MenuItem("VR Tools/Stop Recording")]
        private static void StopRecording()
        {
            if (VRPlayer.SessionPath == null || VRPlayer.SessionPath.Trim().Equals(""))
            {
                EditorUtility.DisplayDialog("VR Recorder",
                "No session is set. Please set a session with VR Tools / Set Session Name",
                "OK");
                return;
            }

            if (EditorApplication.isPlaying == false)
            {
                EditorUtility.DisplayDialog("VR Recorder",
                "Game must be playing before starting recording.",
                "OK");
                return;
            }

            EditorUtility.DisplayDialog("Recorder Mode",
                "VR Recorder Disabled",
                "OK");

            GameObject.Find("[CameraRig]").GetComponent<VRPlayer>().StopRecording();
        }
        [MenuItem("VR Tools/Start Playback")]
        private static void StartPlayback()
        {
            if (VRPlayer.SessionPath == null || VRPlayer.SessionPath.Trim().Equals(""))
            {
                EditorUtility.DisplayDialog("VR Recorder",
                "No session is set. Please set a session with VR Tools / Set Session Name",
                "OK");
                return;
            }

            if (EditorApplication.isPlaying == false)
            {
                EditorUtility.DisplayDialog("VR Recorder",
                "Game must be playing before starting playback.",
                "OK");
                return;
            }

            GameObject.Find("[CameraRig]").GetComponent<VRPlayer>().Play();
        }

        [MenuItem("VR Tools/Set Session Name")]
        private static void SetSessionName()
        {

            if (EditorApplication.isPlaying == false)
            {
                EditorUtility.DisplayDialog("VR Recorder",
                "Game must be playing before setting the session.",
                "OK");
                return;
            }


            var path = EditorUtility.OpenFolderPanel("Select Recorder Session Folder", "", "");

            if (Directory.Exists(path))
            {
                VRPlayer.SessionPath = path;
                EditorUtility.DisplayDialog("VR Recorder",
                "Session Folder Set!\r\n\r\nNow start recording or playback.",
                "OK");
                return;
            }
        }
    }
}
