using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Valve.VR;
using VRTools.Recorder.SteamVR_Extension;

namespace VRTools.Recorder
{
    public class VRPlaybackDevice : MonoBehaviour, ControllerStateProvider
    {
        private LinkedList<VrKeyframe> linkedList;
        private LinkedListNode<VrKeyframe> currentClip;
        private string sessionPath;
        public float playbackSpeed = 1 / 60f;
        private SteamVR_TrackedObject steamVrObject;
        private SteamVR_TrackedController steamVRTrackedController;
        private bool playing;
        private float clipDuration;
        private VRControllerState_t lastControllerState;

        public void Awake()
        {
            linkedList = new LinkedList<VrKeyframe>();
        }

        public void Start()
        {
            SteamVR.enabled = true;
            gameObject.SetActive(true);
            steamVrObject = gameObject.GetComponent<SteamVR_TrackedObject>();
            if (steamVrObject == null) return;
            if (gameObject.name == "Controller (left)")
                steamVrObject.index = SteamVR_TrackedObject.EIndex.Device2;
            if (gameObject.name == "Controller (right)")
                steamVrObject.index = SteamVR_TrackedObject.EIndex.Device3;
            if (gameObject.name == "Camera (eye)")
                steamVrObject.index = SteamVR_TrackedObject.EIndex.Hmd;

            steamVRTrackedController = gameObject.GetComponent<SteamVR_TrackedController>();
            if (steamVRTrackedController != null)
            {
                steamVRTrackedController.SetDeviceIndex((int)steamVrObject.index);
                steamVRTrackedController.controllerStateProvider = this;
            }
            SteamVR_Events.DeviceConnected.Send((int)steamVrObject.index, linkedList.Count > 0);
        }

        public void Update()
        {
            if (!playing) return;
            if (steamVrObject == null) return;
            if (clipDuration == 0f) return;

            float iterCount = Time.time / clipDuration;
            float iterPosition = Time.time / (iterCount);
            iterPosition = 9999;

            if (currentClip == null)
                currentClip = linkedList.First;
            else
            {
                currentClip = currentClip.Next;
            }
            if (currentClip == null)
            {
                return;
            }
            
            TrackedDevicePose_t[] poses = new TrackedDevicePose_t[10];
            TrackedDevicePose_t pose = new TrackedDevicePose_t();
            pose.mDeviceToAbsoluteTracking = new HmdMatrix34_t();
            pose.mDeviceToAbsoluteTracking.m0 = currentClip.Value.m0;
            pose.mDeviceToAbsoluteTracking.m1 = currentClip.Value.m1;
            pose.mDeviceToAbsoluteTracking.m2 = currentClip.Value.m2;
            pose.mDeviceToAbsoluteTracking.m3 = currentClip.Value.m3;
            pose.mDeviceToAbsoluteTracking.m4 = currentClip.Value.m4;
            pose.mDeviceToAbsoluteTracking.m5 = currentClip.Value.m5;
            pose.mDeviceToAbsoluteTracking.m6 = currentClip.Value.m6;
            pose.mDeviceToAbsoluteTracking.m7 = currentClip.Value.m7;
            pose.mDeviceToAbsoluteTracking.m8 = currentClip.Value.m8;
            pose.mDeviceToAbsoluteTracking.m9 = currentClip.Value.m9;
            pose.mDeviceToAbsoluteTracking.m10 = currentClip.Value.m10;
            pose.mDeviceToAbsoluteTracking.m11 = currentClip.Value.m11;
            pose.bDeviceIsConnected = true;
            pose.bPoseIsValid = true;
            poses[(int)steamVrObject.index] = pose;
            steamVrObject.OnNewPoses(poses);
            lastControllerState = currentClip.Value.controllerState;

        }

        public VRPlaybackDevice SetSessionPath(string sessionName)
        {
            this.sessionPath = sessionName;
            load(sessionPath + @"\" + gameObject.name + ".txt");
            return this;
        }

        private void load(string trackerDataFile)
        {
            if (new FileInfo(trackerDataFile).Exists == false) return;

            using (FileStream fs = File.OpenRead(trackerDataFile))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] parts = line.Split('#');
                        if (parts[0].StartsWith("("))
                        {
                            VrKeyframe frame = new VrKeyframe();
                            frame.name = parts[0].Trim();
                            if (frame.name.StartsWith("(") && frame.name.EndsWith(")"))
                                frame.name = frame.name.Substring(1, frame.name.Length - 2);

                            String timeStr = parts[1].Trim();
                            if (timeStr.StartsWith("(") && timeStr.EndsWith(")"))
                                timeStr = timeStr.Substring(1, timeStr.Length - 2);

                            frame.time = Convert.ToSingle(timeStr);
                            if (clipDuration < frame.time)
                                clipDuration = frame.time;

                            String[] mBits = parts[2].Trim().Split('/');
                            frame.m0 = Convert.ToSingle(mBits[0]);
                            frame.m1 = Convert.ToSingle(mBits[1]);
                            frame.m2 = Convert.ToSingle(mBits[2]);
                            frame.m3 = Convert.ToSingle(mBits[3]);
                            frame.m4 = Convert.ToSingle(mBits[4]);
                            frame.m5 = Convert.ToSingle(mBits[5]);
                            frame.m6 = Convert.ToSingle(mBits[6]);
                            frame.m7 = Convert.ToSingle(mBits[7]);
                            frame.m8 = Convert.ToSingle(mBits[8]);
                            frame.m9 = Convert.ToSingle(mBits[9]);
                            frame.m10 = Convert.ToSingle(mBits[10]);
                            frame.m11 = Convert.ToSingle(mBits[11]);

                            frame.controllerState.ulButtonPressed = Convert.ToUInt64(mBits[12]);
                            frame.controllerState.ulButtonTouched = Convert.ToUInt64(mBits[13]);
                            frame.controllerState.rAxis0.x = Convert.ToSingle(mBits[14]);
                            frame.controllerState.rAxis0.y = Convert.ToSingle(mBits[15]);

                            frame.controllerState.rAxis1.x = Convert.ToSingle(mBits[16]);
                            frame.controllerState.rAxis1.y = Convert.ToSingle(mBits[17]);

                            frame.controllerState.rAxis2.x = Convert.ToSingle(mBits[18]);
                            frame.controllerState.rAxis2.y = Convert.ToSingle(mBits[19]);

                            frame.controllerState.rAxis3.x = Convert.ToSingle(mBits[20]);
                            frame.controllerState.rAxis3.y = Convert.ToSingle(mBits[21]);

                            frame.controllerState.rAxis4.x = Convert.ToSingle(mBits[22]);
                            frame.controllerState.rAxis4.y = Convert.ToSingle(mBits[23]);

                            linkedList.AddLast(frame);
                        }
                    }
                }
            }


        }


        public void Play()
        {
            playing = true;
        }

        public VRPlaybackDevice SetSpeed(float f)
        {
            this.playbackSpeed = f;
            return this;
        }

        public bool getControllerState(uint controllerIndex, ref VRControllerState_t controllerState)
        {
            controllerState.ulButtonPressed = lastControllerState.ulButtonPressed;
            controllerState.ulButtonTouched = lastControllerState.ulButtonTouched;
            controllerState.rAxis0 = lastControllerState.rAxis0;
            controllerState.rAxis1 = lastControllerState.rAxis1;
            controllerState.rAxis2 = lastControllerState.rAxis2;
            controllerState.rAxis3 = lastControllerState.rAxis3;
            controllerState.rAxis4 = lastControllerState.rAxis4;
            return true;
        }

    }
    public class VrKeyframe
    {
        public String name;
        public float time;
        public float m0; //float[3][4]
        public float m1;
        public float m2;
        public float m3;
        public float m4;
        public float m5;
        public float m6;
        public float m7;
        public float m8;
        public float m9;
        public float m10;
        public float m11;
        public VRControllerState_t controllerState;
    }

}
