using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTools.Recorder.SteamVR_Extension;

namespace VRTools.Recorder
{
    public class TrackedObjectRecorderProxy : MonoBehaviour
    {
        private bool recording;
        private SteamVR_TrackedObject trackingObject;
        // Use this for initialization
        void Start()
        {
        }

        void Awake()
        {
            this.trackingObject = gameObject.GetComponent<SteamVR_TrackedObject>();
            trackingObject.SetDelegate(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        VRControllerState_t state, prevState;
        public bool OnNewPoses(TrackedDevicePose_t[] poses)
        {
            // return true to override
            if (GetComponent<VRRecorder>() == null) return false;

            if (GetComponent<VRRecorder>().started)
            {
                if (trackingObject.index == SteamVR_TrackedObject.EIndex.None)
                    return false;

                var i = (int)trackingObject.index;

                var trackingInfo = poses[i].mDeviceToAbsoluteTracking;

                // Get controller state
                var system = OpenVR.System;
                if (system != null)
                {
                    bool valid = system.GetControllerState((uint)i, ref state,
                        (uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(VRControllerState_t)));
                    
                }

                GetComponent<VRRecorder>().addPose(gameObject.name, Time.time, trackingInfo, state);
                return false;
            }


            return true;
        }
    }
    
    

}