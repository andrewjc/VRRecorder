using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using Valve.VR;
using ThreadPriority = System.Threading.ThreadPriority;

namespace VRTools.Recorder
{
    public class VRRecorder : MonoBehaviour
    {
        public bool started;
        private bool stopTracking;

        public ArrayList trackerArray = new ArrayList(100000);

        public static float RECORD_FREQUENCY = 0.01f;
        
        protected VRRecorder()
        {
        }

        public void StartRecording()
        {
            started = true;
        }

        public void StopRecording()
        {
            stopTracking = true;
            using (FileStream file = File.OpenWrite(sessionPath + "/" + gameObject.name +".txt"))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    foreach (String str in trackerArray)
                    {
                        sw.WriteLine(str);
                    }
                }
            }
        }

        private VrKeyframe currentKeyframe;
        private string sessionPath;

        public void addPose(string gameObjectName, float time, HmdMatrix34_t trackingInfo, VRControllerState_t controllerState)
        {
            if (!started) return;

            bool populated = false;
            while (!populated)
            {
                if (currentKeyframe == null || time > currentKeyframe.time + RECORD_FREQUENCY)
                {
                    if(currentKeyframe == null)
                        currentKeyframe = new VrKeyframe();
                    currentKeyframe.time = currentKeyframe == null ? 0 : currentKeyframe.time + RECORD_FREQUENCY;
                    currentKeyframe.m0 = trackingInfo.m0;
                    currentKeyframe.m1 = trackingInfo.m1;
                    currentKeyframe.m2 = trackingInfo.m2;
                    currentKeyframe.m3 = trackingInfo.m3;
                    currentKeyframe.m4 = trackingInfo.m4;
                    currentKeyframe.m5 = trackingInfo.m5;
                    currentKeyframe.m6 = trackingInfo.m6;
                    currentKeyframe.m7 = trackingInfo.m7;
                    currentKeyframe.m8 = trackingInfo.m8;
                    currentKeyframe.m9 = trackingInfo.m9;
                    currentKeyframe.m10 = trackingInfo.m10;
                    currentKeyframe.m11 = trackingInfo.m11;
                    currentKeyframe.controllerState = controllerState;
                    populated = true;
                    
                    String str = String.Format("({13}) # ({0}) # {1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}/{14}/{15}/{16}/{17}/{18}/{19}/{20}/{21}/{22}/{23}/{24}/{25}",
                       currentKeyframe.time,
                        currentKeyframe.m0,
                        currentKeyframe.m1,
                        currentKeyframe.m2,
                        currentKeyframe.m3,
                        currentKeyframe.m4,
                        currentKeyframe.m5,
                        currentKeyframe.m6,
                        currentKeyframe.m7,
                        currentKeyframe.m8,
                        currentKeyframe.m9,
                        currentKeyframe.m10,
                        currentKeyframe.m11,

                        currentKeyframe.controllerState.ulButtonPressed,
                        currentKeyframe.controllerState.ulButtonTouched,
                        currentKeyframe.controllerState.rAxis0.x,
                        currentKeyframe.controllerState.rAxis0.y,
                        currentKeyframe.controllerState.rAxis1.x,
                        currentKeyframe.controllerState.rAxis1.y,
                        currentKeyframe.controllerState.rAxis2.x,
                        currentKeyframe.controllerState.rAxis2.y,
                        currentKeyframe.controllerState.rAxis3.x,
                        currentKeyframe.controllerState.rAxis3.y,
                        currentKeyframe.controllerState.rAxis4.x,
                        currentKeyframe.controllerState.rAxis4.y,
                        
                        gameObjectName
                        );
                    trackerArray.Add(str);
                }
                else populated = true;
            }
        }

        public VRRecorder SetSessionPath(string sessionName)
        {
            this.sessionPath = sessionName;
            return this;
        }
    }

 
}