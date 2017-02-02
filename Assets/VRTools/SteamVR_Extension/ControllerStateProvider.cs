using Valve.VR;

namespace VRTools.Recorder.SteamVR_Extension
{
    public interface ControllerStateProvider
    {
        bool getControllerState(uint controllerIndex, ref VRControllerState_t controllerState);
    }
}