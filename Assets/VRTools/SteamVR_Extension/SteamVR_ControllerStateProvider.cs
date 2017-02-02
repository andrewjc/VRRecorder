using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valve.VR;
using VRTools.Recorder.SteamVR_Extension;

namespace VRTools.Recorder.SteamVR_Extension
{
    class SteamVR_ControllerStateProvider : ControllerStateProvider
    {
        public bool getControllerState(uint controllerIndex, ref VRControllerState_t controllerState)
        {
            var system = OpenVR.System;
            return system != null &&
                   system.GetControllerState(controllerIndex, ref controllerState,
                       (uint) System.Runtime.InteropServices.Marshal.SizeOf(typeof(VRControllerState_t)));
        }
    }
}
