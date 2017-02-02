using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VRTools.Recorder
{
    public class VRPlayer : MonoBehaviour
    {
        private string trackerDataFile;
        private LinkedList<VrKeyframe> linkedList;
        public float playbackSpeed = 1/60f;
        private LinkedListNode<VrKeyframe> currentClip;

        public GameObject leftController;
        public GameObject rightController;
        public GameObject hmdDevice;
        public bool playbackMode = true;
        public static string SessionPath { get; set; }

        public void Awake()
        {
            linkedList = new LinkedList<VrKeyframe>();
        }

        public void Start()
        {
           /* if (playbackMode)
                SteamVR.enabled = false;*/
        }
        
        public void StartRecording()
        {
            // Add a recorder to each tracked object
            if(hmdDevice != null)
                hmdDevice.gameObject.AddComponent<TrackedObjectRecorderProxy>().gameObject.AddComponent<VRRecorder>().SetSessionPath(SessionPath).StartRecording();
            if(leftController != null)
                leftController.gameObject.AddComponent<TrackedObjectRecorderProxy>().gameObject.AddComponent<VRRecorder>().SetSessionPath(SessionPath).StartRecording();
            if(rightController != null)
                rightController.gameObject.AddComponent<TrackedObjectRecorderProxy>().gameObject.AddComponent<VRRecorder>().SetSessionPath(SessionPath).StartRecording();
        }

        public void StopRecording()
        {
            if (hmdDevice != null)
                hmdDevice.gameObject.GetComponent<VRRecorder>().SetSessionPath(SessionPath).StopRecording();
            if (leftController != null)
                leftController.gameObject.GetComponent<VRRecorder>().SetSessionPath(SessionPath).StopRecording();
            if (rightController != null)
                rightController.gameObject.GetComponent<VRRecorder>().SetSessionPath(SessionPath).StopRecording();
        }
        
        public VRPlayer Play()
        {
            SteamVR.enabled = false;
            if (leftController != null)
            {
                leftController.SetActive(true);
                var renderModel = leftController.gameObject.GetComponentInChildren<SteamVR_RenderModel>();
                if(renderModel != null)
                    renderModel.StartCoroutine(renderModel.SetModelAsync("vr_controller_vive_1_5"));
                leftController.gameObject.AddComponent<VRPlaybackDevice>().SetSpeed(playbackSpeed).SetSessionPath(SessionPath).Play();
            }

            if (rightController != null)
            {
                rightController.SetActive(true);
                var renderModel2 = rightController.gameObject.GetComponentInChildren<SteamVR_RenderModel>();
                if(renderModel2 != null)
                    renderModel2.StartCoroutine(renderModel2.SetModelAsync("vr_controller_vive_1_5"));
                rightController.gameObject.AddComponent<VRPlaybackDevice>().SetSpeed(playbackSpeed).SetSessionPath(SessionPath).Play();
            }
            if (hmdDevice != null)
            {
                hmdDevice.SetActive(true);
                if(hmdDevice.GetComponent<SteamVR_Camera>() != null)
                    hmdDevice.GetComponent<SteamVR_Camera>().enabled = false;

                hmdDevice.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
                hmdDevice.GetComponent<Camera>().fieldOfView = 60f;

                hmdDevice.gameObject.AddComponent<VRPlaybackDevice>().SetSpeed(playbackSpeed).SetSessionPath(SessionPath).Play();
            }

            return this;
        }
    }

}
